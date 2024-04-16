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
        const int FLOP = 1, TURN = 2, RIVER = 3;
        bool ended = false;
        private int buyIn, smallBlind, bigBlind;
        private List<MenuWindow> users;
        private Deck deck; // all the cards that are not in the players hands
        DBService dbService;
        Random random;

        private Card[] communityCards;
        public TableService(int buyIn, int smallBlind, int bigBlind, string tableType , DBService dbService)
        {
            this.dbService = dbService;
            this.tableType = tableType;
            users = new List<MenuWindow>();

            this.buyIn = buyIn;
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;

            communityCards = new Card[5];

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

        public Card generateCard()
        {
            return getRandomCardAndRemoveIt();
            // notify_all();
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
                mutex.ReleaseMutex();
                if (activePlayers.Count == 0)
                {
                    await Task.Delay(5000);
                    continue;
                }
                deck = new Deck();
                foreach (MenuWindow window in activePlayers) { window.resetCards(tableType); }
                await Task.Delay(1000);
                /// give first card
                foreach (MenuWindow playerWindow in activePlayers)
                {
                    User player = playerWindow.Player();
                    Card card = generateCard();
                    player.UserCurrentHand[0] = card; 
                    foreach (MenuWindow window in activePlayers) { window.notify_card(tableType, player, 1, card.Value + card.Suit); }
                    await Task.Delay(400);
                }
                /// give second card
                foreach (MenuWindow playerWindow in activePlayers)
                {
                    User player = playerWindow.Player();
                    Card card = generateCard();
                    player.UserCurrentHand[1] = card;
                    foreach (MenuWindow window in activePlayers) { window.notify_card(tableType, player, 2, card.Value + card.Suit); }
                    await Task.Delay(400);
                }

                for (int turn = 1; turn <= 3; turn++)
                {
                    
                    if (turn == FLOP)
                    {
                        for (int cardNumber = 0; cardNumber < 3; cardNumber++)
                        {
                            communityCards[cardNumber] = generateCard();
                        }
                    }
                    else if (turn == TURN)
                        communityCards[3] = generateCard();
                    else if (turn == RIVER)
                        communityCards[4] = generateCard();

                    bool turnEnded = false;
                    int currentBet = -1;
                    int currentBetPlayer = -1;

                    while (!turnEnded)
                    {
                        if (activePlayers.Count == 0) break;
                        MenuWindow currentWindow = activePlayers.Dequeue();

                        User player = currentWindow.Player();

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
            player.UserTablePlace = users.Count;
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
