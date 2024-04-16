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

        const string HEART = "H", DIAMOND = "D", SPADE = "S", CLUB = "C";
        private string tableType;
        private static Mutex mutex;
        const int FULL = 8, EMPTY = 0, WAITING = 1;
        bool ended = false;
        private int buyIn, smallBlind, bigBlind;
        private List<MenuWindow> users;
        private List<Card> tableDeck; // all the cards that are not in the players hands

        private List<Card> communityCards;
        public TableService(int buyIn, int smallBlind, int bigBlind, string tableType)
        {
            this.tableType = tableType;
            users = new List<MenuWindow>();

            this.buyIn = buyIn;
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;

            Task.Run(() => runTable());

            mutex = new Mutex();
        }

        public void addCardsForValue(string value)
        {
            tableDeck.Add(new Card(value, HEART));
            tableDeck.Add(new Card(value, DIAMOND));
            tableDeck.Add(new Card(value, SPADE));
            tableDeck.Add(new Card(value, CLUB));
        }


        public void generateAllPossibleHands()
        {
            tableDeck = new List<Card>();

            for (int cardValue = 2; cardValue <= 10; cardValue++)
                addCardsForValue(i.ToString());

            addCardsForValue("J");
            addCardsForValue("Q");
            addCardsForValue("K");
            addCardsForValue("A");
        }

        public Card getRandomCardAndRemoveIt()
        {
            Random random = new Random();
            int index = random.Next(0, tableDeck.Count);
            Card card = new Card(tableDeck[index].Value, tableDeck[index].Suit);
            tableDeck.RemoveAt(index);
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
                generateAllPossibleHands();
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
                    for (int i = 0; i < 5; ++i)
                    {
                        Console.WriteLine(communityCards[i].Value + " " + communityCards[i].Suit);
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
                        for (int i = 0; i < 2; ++i)
                        {
                            Console.WriteLine(player.UserCurrentHand[i].Value + " " + player.UserCurrentHand[i].Suit);
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

            SqlCommand updateChips = new SqlCommand("EXEC updateUserChips @userID , @userChips", sqlConnection);
            updateChips.Parameters.AddWithValue("@userID", player.UserID);
            updateChips.Parameters.AddWithValue("@userChips", player.UserChips - buyIn);
            updateChips.ExecuteNonQuery();

            SqlCommand resetStack = new SqlCommand("EXEC updateUserStack @userID , @userStack", sqlConnection);
            resetStack.Parameters.AddWithValue("@userID", player.UserID);
            resetStack.Parameters.AddWithValue("@userStack", buyIn);
            resetStack.ExecuteNonQuery();

            player.UserChips -= buyIn;
            player.UserStack = buyIn;
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
