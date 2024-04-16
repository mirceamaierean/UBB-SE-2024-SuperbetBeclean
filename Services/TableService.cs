using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SuperbetBeclean.Model;
using SuperbetBeclean.Windows;

namespace SuperbetBeclean.Services
{
    public class TableService
    {

        private string tableType;
        private static Mutex mutex;
        const int FULL = 8, EMPTY = 0, WAITING = 1;
        bool ended = false;
        private int buyIn, smallBlind, bigBlind;
        private List<MenuWindow> users;
        private Deck deck; // all the cards that are not in the players hands
        DBService dbService;
        Random random;

        private List<Card> communityCards;
        public TableService(int buyIn, int smallBlind, int bigBlind, string tableType , DBService dbService)
        {
            this.dbService = dbService;
            this.tableType = tableType;
            users = new List<MenuWindow>();

            this.buyIn = buyIn;
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;

            Task.Run(() => runTable());

            mutex = new Mutex();

            random = new Random();
            this.dbService = dbService;
        }

        public Card getRandomCardAndRemoveIt()
        {
            int index = random.Next(0, deck.getDeckSize());
            Card card = deck.getCardFromIndex(index);
            deck.removeCardFromIndex(index);
            return card;
        }

        public void generateCommunityCards()
        {
            communityCards = new List<Card>();
            for (int i = 0; i < 5; i++)
                communityCards.Add(getRandomCardAndRemoveIt());
        }

        public Card[] generateUserCards()
        {
            Card[] userCards = new Card[2];
            userCards[0] = getRandomCardAndRemoveIt();
            userCards[1] = getRandomCardAndRemoveIt();
            return userCards;
        }

        public async void runTable()
        {
            while (true)
            {
                if (ended) break;
                if (isEmpty())
                {
                    await Task.Delay(3000);
                    continue;
                }
                mutex.WaitOne();
                // start new round with all players
                Queue<MenuWindow> activePlayers = new Queue<MenuWindow>(users);
                if (activePlayers.Count == 0)
                {
                    mutex.ReleaseMutex();
                    await Task.Delay(3000);
                    continue;
                }
                deck = new Deck();
                // generate community cards
                generateCommunityCards();
                // add cards to players
                foreach (MenuWindow window in activePlayers)
                {
                    Card[] userCards = generateUserCards();
                    User player = window.Player();
                    player.UserCurrentHand = userCards;
                }

                mutex.ReleaseMutex();
                for (int i = 1; i <= 3; i++)
                {
                    Console.WriteLine("We are in turn " + i + "!");
                    // display community cards
                    Console.WriteLine("Community cards are:");
                    for (int cardValue = 0; cardValue < 5; ++cardValue)
                    {
                        Console.WriteLine(communityCards[cardValue].Value + " " + communityCards[cardValue].Suit);
                    }
                    bool turnEnded = false;
                    int currentBet = -1;
                    int currentBetPlayer = -1;

                    while (!turnEnded)
                    {
                        if (activePlayers.Count == 0) { break; }
                        MenuWindow currentWindow = activePlayers.Dequeue();

                        User player = currentWindow.Player();

                        // display player cards
                        Console.WriteLine("Player cards are:");
                        for (int index = 0; index < 2; ++index)
                        {
                            Console.WriteLine(player.UserCurrentHand[index].Value + " " + player.UserCurrentHand[index].Suit);
                        }

                        if (player.UserStatus == 0) continue;
                        activePlayers.Enqueue(currentWindow);
                        if (player.UserID == currentBetPlayer) break;
                        int playerBet = await currentWindow.startTime(tableType);
                        if (playerBet > currentBet)
                        {
                            currentBet = playerBet;
                            currentBetPlayer = player.UserID;
                        }
                        foreach (MenuWindow window in activePlayers) { window.notify(tableType, currentBet); }
                    }
                    foreach (MenuWindow currentWindow in activePlayers) { currentWindow.endTurn(tableType); }
                }
            }
        }

        public void disconnectUser(MenuWindow window)
        {
            mutex.WaitOne();
            users.Remove(window);
            mutex.ReleaseMutex();
        }

        public bool joinTable(MenuWindow window, ref SqlConnection sqlConnection)
        {
            if (isFull()) return false;

            User player = window.Player();

            if (player.UserChips < buyIn) return false; /// also return different values to differentiate full from no money

            player.UserChips -= buyIn;
            dbService.UpdateUserChips(player.UserID, player.UserChips);

            player.UserStack = buyIn;
            dbService.UpdateUserStack(player.UserID, player.UserStack);

            player.UserStatus = WAITING;

            mutex.WaitOne();
            users.Add(window);
            mutex.ReleaseMutex();

            return true;
        }

        public bool isFull()
        {
            return users.Count == FULL;
        }

        public bool isEmpty()
        {
            return users.Count == EMPTY;
        }

        public int occupied()
        {
            return users.Count;
        }
    }
}
