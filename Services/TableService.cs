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
                Queue<MenuWindow> activePlayers = new Queue<MenuWindow>(users);
                mutex.ReleaseMutex();
                for (int i = 1; i <= 3; i++)
                {
                    Console.WriteLine("We are in turn " + i + "!");
                    bool turnEnded = false;
                    int currentBet = -1;
                    int currentBetPlayer = -1;

                    while (!turnEnded)
                    {
                        if (activePlayers.Count == 0) { break; }
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
