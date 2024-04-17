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
        const int FULL = 8, EMPTY = 0;
        const int PREFLOP = 0, FLOP = 1, TURN = 2, RIVER = 3;
        const int INACTIVE = 0, WAITING = 1, FOLDED = 2, PLAYING = 3;
        bool ended = false;
        private int buyIn, smallBlind, bigBlind;
        private List<MenuWindow> users;
        private Deck deck; // all the cards that are not in the players hands
        DBService dbService;
        Random random;
        HandRankCalculator rankCalculator;

        private Card[] communityCards;
        private int[] freeSpace;
        public TableService(int buyIn, int smallBlind, int bigBlind, string tableType, DBService dbService)
        {
            this.dbService = dbService;
            this.tableType = tableType;
            users = new List<MenuWindow>();
            rankCalculator = new HandRankCalculator();

            this.buyIn = buyIn;
            this.smallBlind = smallBlind;
            this.bigBlind = bigBlind;

            communityCards = new Card[6];
            freeSpace = new int[9];

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
                foreach (MenuWindow menuWindow in activePlayers)
                {
                    User player = menuWindow.Player();
                    player.UserStatus = PLAYING;
                    player.UserBet = 0;
                }
                foreach (MenuWindow currentWindow in activePlayers)
                {
                    User currentPlayer = currentWindow.Player();
                    foreach (MenuWindow otherWindow in activePlayers)
                    {
                        User otherPlayer = otherWindow.Player();
                        currentWindow.startRound(tableType, otherPlayer);
                    }
                }
                foreach (MenuWindow window in activePlayers)
                {
                    window.resetCards(tableType);
                }
                if (activePlayers.Count < 2)
                {
                    await Task.Delay(5000);
                    continue;
                }
                deck = new Deck();
                
                await Task.Delay(1000);
                /// give first card
                foreach (MenuWindow playerWindow in activePlayers)
                {
                    User player = playerWindow.Player();
                    Card card = generateCard();
                    player.UserCurrentHand[0] = card;
                    foreach (MenuWindow window in activePlayers) { window.notifyUserCard(tableType, player, 1, card.Value + card.Suit); }
                    await Task.Delay(400);
                }
                /// give second card
                foreach (MenuWindow playerWindow in activePlayers)
                {
                    User player = playerWindow.Player();
                    Card card = generateCard();
                    player.UserCurrentHand[1] = card;
                    foreach (MenuWindow window in activePlayers) { window.notifyUserCard(tableType, player, 2, card.Value + card.Suit); }
                    await Task.Delay(400);
                }
                for (int i=1;i<=5;i++)
                {
                    Card card = generateCard();
                    communityCards[i] = card;
                }
                int tablePot = 0;
                for (int turn = 0; turn <= 3; turn++)
                {
                    if (activePlayers.Count < 2) break;
                    if (turn == PREFLOP)
                    {
                    }
                    else if (turn == FLOP)
                    {
                        for (int cardNumber = 1; cardNumber <= 3; cardNumber++)
                        {
                            foreach (MenuWindow window in activePlayers) { window.notifyTableCard(tableType, cardNumber, communityCards[cardNumber].Info()); }
                            await Task.Delay(400);
                        }
                    }
                    else if (turn == TURN)
                    {
                        foreach (MenuWindow window in activePlayers) { window.notifyTableCard(tableType, 4, communityCards[4].Info()); }
                        await Task.Delay(400);
                    }
                    else if (turn == RIVER)
                    {
                        foreach (MenuWindow window in activePlayers) { window.notifyTableCard(tableType, 5, communityCards[5].Info()); }
                        await Task.Delay(400);
                    }
                    bool turnEnded = false;
                    int currentBet = -1;
                    int currentBetPlayer = -1;
                    while (!turnEnded)
                    {
                        if (activePlayers.Count < 2) break;
                        MenuWindow currentWindow = activePlayers.Peek();
                        User player = currentWindow.Player();

                        if (player.UserStatus != PLAYING)
                        {
                            activePlayers.Dequeue();
                            continue;
                        }
                        if (player.UserID == currentBetPlayer) break;
                        int playerBet = await currentWindow.startTime(tableType, currentBet, player.UserStack);
                        if (playerBet == -1)
                        {
                            activePlayers.Dequeue();
                            Console.WriteLine(player.UserName + " folded!");
                            player.UserStatus = WAITING;
                            player.UserBet = 0;
                            foreach (MenuWindow window in activePlayers) { window.foldPlayer(tableType, player); }
                        }
                        else
                        {
                            activePlayers.Enqueue(activePlayers.Dequeue());
                            int extraBet = playerBet - player.UserBet;
                            player.UserStack -= extraBet;
                            tablePot += extraBet;
                            dbService.UpdateUserStack(player.UserID, player.UserStack);
                            player.UserBet = playerBet;
                            if (playerBet > currentBet)
                            {
                                currentBet = playerBet;
                                currentBetPlayer = player.UserID;
                            }
                        }
                        foreach (MenuWindow window in activePlayers) { window.notify(tableType, player, tablePot, currentBet); }
                    }
                    foreach (MenuWindow currentWindow in activePlayers) {
                        User currentPlayer = currentWindow.Player();
                        foreach (MenuWindow otherWindow in activePlayers)
                        {
                            User otherPlayer = otherWindow.Player();
                            currentWindow.endTurn(tableType, otherPlayer);
                        }
                        currentPlayer.UserBet = 0;
                    }
                }
                foreach (MenuWindow currentWindow in activePlayers)
                {
                    User currentPlayer = currentWindow.Player();
                    foreach (MenuWindow otherWindow in activePlayers)
                    {
                        User otherPlayer = otherWindow.Player();
                        currentWindow.showCards(tableType, otherPlayer);
                    }
                }
                List<User> winners = determineWinners(activePlayers);
                foreach(User winner in winners)
                {
                    Console.WriteLine("Winner: " + winner.UserName);
                    winner.UserStack += Convert.ToInt32(tablePot / winners.Count);
                    dbService.UpdateUserStack(winner.UserID, winner.UserStack);
                }
                await Task.Delay(5000);
                
            }
        }

        private void generateHands(List < Card > currentHand, List<Card> possibleCards, int lastCard, int numberCards, List<List<Card>> allHands)
        {
            for (int i=lastCard+1; i<possibleCards.Count; i++)
            {
                currentHand.Add(possibleCards[i]);
                if (currentHand.Count == numberCards)
                {
                    List < Card > handCopy = new List<Card> (currentHand);
                    allHands.Add(handCopy);
                }
                else
                {
                    generateHands(currentHand, possibleCards, i, numberCards, allHands);
                }
                currentHand.Remove(possibleCards[i]);
            }
        }

        Tuple < int, int > determineMaxHand(List < Card > possibleCards)
        {
            Tuple<int, int> maxHandValue = new Tuple<int, int>(0, 0);
            List < List < Card >> allHands = new List<List<Card>> ();
            List < Card > currentHand = new List<Card> ();
            generateHands(currentHand, possibleCards, -1, 5, allHands);
            Console.WriteLine("Generated hands: " + allHands.Count);
            foreach (List<Card> hand in allHands)
            {
                Tuple<int, int> handValue = rankCalculator.getValue(hand);
                if (handValue.Item1 > maxHandValue.Item1 || (handValue.Item1 == maxHandValue.Item1 && handValue.Item2 > maxHandValue.Item2))
                {
                    maxHandValue = handValue;
                }
            }
            Console.WriteLine("Best hand: " + maxHandValue);
            return maxHandValue;
        }

        public List < User > determineWinners(Queue < MenuWindow > activePlayers)
        {
            Tuple < int , int > maxHand = new Tuple<int, int> (0, 0);
            List<User> winners = new List<User>();
            Dictionary<User, Tuple<int, int>> results = new Dictionary<User, Tuple<int, int>>();
            foreach (MenuWindow window in activePlayers)
            {
                User player = window.Player();
                List< Card > possibleCards = new List<Card>();
                for (int i=1;i<=5;i++)
                {
                    possibleCards.Add(communityCards[i]);
                }
                possibleCards.Add(player.UserCurrentHand[0]);
                possibleCards.Add(player.UserCurrentHand[1]);
                Tuple <int, int > hand = determineMaxHand(possibleCards);
                if (hand.Item1 > maxHand.Item1 || (hand.Item1 == maxHand.Item1 && hand.Item2 > maxHand.Item2))
                {
                    maxHand = hand;
                    winners.Clear();
                    winners.Add(player);
                }
                else if (hand.Item1 == maxHand.Item1 && hand.Item2 == maxHand.Item2)
                {
                    winners.Add(player);
                }
            }
            return winners;
        }

    public void disconnectUser(MenuWindow window)
        {
            User player = window.Player();
            freeSpace[player.UserTablePlace] = 0;
            mutex.WaitOne();
            foreach (MenuWindow windowUser in users)
            {
                windowUser.foldPlayer(tableType, player);
                windowUser.hidePlayer(tableType, player);
            }
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
            for (int i=1;i<=FULL;i++)
            {
                if (freeSpace[i] == 0)
                {
                    freeSpace[i] = 1;
                    player.UserTablePlace = i;
                    break;
                }
            }
            mutex.WaitOne();
            users.Add(window);
            foreach (MenuWindow windowUser in users)
            {
                windowUser.showPlayer(tableType, player);
                window.showPlayer(tableType, windowUser.Player());
            }
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
