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
    public class Service
    {
        private static Mutex internMutex, juniorMutex, seniorMutex;
        private List<MenuWindow> openedUsersWindows;
        private SqlConnection sqlConnection;
        const int FULL = 8, EMPTY = 0;
        bool ended = false;
        const int INACTIVE = 0, WAITING = 1, PLAYING = 2;
        private Dictionary<string, int> buyIn, smallBlind, bigBlind;
        private List<MenuWindow> internTableUsers;
        private List<MenuWindow> juniorTableUsers;
        private List<MenuWindow> seniorTableUsers;
        string connectionString;
        // Task internTask, juniorTask, seniorTask;

        public Service()
        {
            connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
            openedUsersWindows = new List<MenuWindow>();
            internTableUsers = new List<MenuWindow>();
            juniorTableUsers = new List<MenuWindow>();
            seniorTableUsers = new List<MenuWindow>();
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            buyIn = new Dictionary<string, int>();
            smallBlind = new Dictionary<string, int>();
            bigBlind = new Dictionary<string, int>();
            buyIn["Intern"] = 5000; buyIn["Junior"] = 50000; buyIn["Senior"] = 500000;
            smallBlind["Intern"] = 50; smallBlind["Junior"] = 500; smallBlind["Senior"] = 5000;
            bigBlind["Intern"] = 100; bigBlind["Junior"] = 1000; bigBlind["Senior"] = 10000;
            Task.Run(() => runInternTable());
            Task.Run(() => runJuniorTable());
            Task.Run(() => runSeniorTable());
            internMutex = new Mutex(); juniorMutex = new Mutex(); seniorMutex = new Mutex();
        }


        private async void runInternTable()
        {
            while (true)
            {
                if (ended) break;
                if (isInternEmpty())
                {
                    await Task.Delay(3000);
                    Console.WriteLine("intern has no players :(");
                    continue;
                }
                internMutex.WaitOne();
                Queue<MenuWindow> activePlayers = new Queue<MenuWindow>(internTableUsers);
                internMutex.ReleaseMutex();
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
                        int playerBet = await currentWindow.startTime("intern");
                        if (playerBet > currentBet)
                        {
                            currentBet = playerBet;
                            currentBetPlayer = player.UserID;
                        }
                        foreach (MenuWindow window in activePlayers) { window.notify("intern", currentBet); }
                    }
                    foreach (MenuWindow currentWindow in activePlayers) { currentWindow.endTurn("intern"); }

                }
                // internTableUsers.Clear();
                // Console.WriteLine("we have a player!!!");
                // Thread.Sleep(3000);
            }
        }

        private async void runJuniorTable()
        {
            while (true)
            {
                if (ended) break;
                if (isJuniorEmpty())
                {
                    await Task.Delay(3000);
                    Console.WriteLine("junior has no players :(");
                    continue;
                }
                juniorMutex.WaitOne();
                Queue<MenuWindow> activePlayers = new Queue<MenuWindow>(juniorTableUsers);
                juniorMutex.ReleaseMutex();
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
                        int playerBet = await currentWindow.startTime("junior");
                        if (playerBet > currentBet)
                        {
                            currentBet = playerBet;
                            currentBetPlayer = player.UserID;
                            activePlayers.Enqueue(currentWindow);
                        }
                        else if (currentBetPlayer == player.UserID) turnEnded = true;
                        else
                        {
                            activePlayers.Enqueue(currentWindow);
                        }
                        foreach (MenuWindow window in activePlayers) { window.notify("junior", currentBet); }
                    }
                    foreach (MenuWindow currentWindow in activePlayers) { currentWindow.endTurn("junior"); }

                }
                juniorTableUsers.Clear();
                // Console.WriteLine("we have a player!!!");
                // Thread.Sleep(3000);
            }
        }

        private async Task runSeniorTable()
        {
            while (true)
            {
                if (ended) break;
                if (isSeniorEmpty())
                {
                    await Task.Delay(3000);
                    Console.WriteLine("senior has no players :(");
                    continue;
                }
                seniorMutex.WaitOne();
                Queue<MenuWindow> activePlayers = new Queue<MenuWindow>(seniorTableUsers);
                seniorMutex.ReleaseMutex();
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
                        int playerBet = await currentWindow.startTime("senior");
                        if (playerBet > currentBet)
                        {
                            currentBet = playerBet;
                            currentBetPlayer = player.UserID;
                            activePlayers.Enqueue(currentWindow);
                        }
                        else if (currentBetPlayer == player.UserID) turnEnded = true;
                        else
                        {
                            activePlayers.Enqueue(currentWindow);
                        }
                        foreach (MenuWindow window in activePlayers) { window.notify("senior", currentBet); }
                    }
                    foreach (MenuWindow currentWindow in activePlayers) { currentWindow.endTurn("senior"); }

                }
                seniorTableUsers.Clear();
                // Console.WriteLine("we have a player!!!");
                // Thread.Sleep(3000);
            }
        }

        public void newUserLogin(User newUser)
        {
            if (DateTime.Now.Date != newUser.UserLastLogin.Date)
            {
                var diffDates = DateTime.Now.Date - newUser.UserLastLogin.Date;
                if (diffDates.Days == 1)
                {
                    newUser.UserStreak++;
                }
                else
                {
                    newUser.UserStreak = 1;
                }
                newUser.UserChips += newUser.UserStreak * 5000;
                SqlCommand dailyBonusCmd = new SqlCommand("EXEC updateUserChips @userID , @userChips", sqlConnection);
                dailyBonusCmd.Parameters.AddWithValue("@userID", newUser.UserID);
                dailyBonusCmd.Parameters.AddWithValue("@userChips", newUser.UserChips);
                dailyBonusCmd.ExecuteNonQuery();
                SqlCommand newStreakCmd = new SqlCommand("EXEC updateUserStreak @userID , @userStreak", sqlConnection);
                newStreakCmd.Parameters.AddWithValue("@userID", newUser.UserID);
                newStreakCmd.Parameters.AddWithValue("@userStreak", newUser.UserStreak);
                newStreakCmd.ExecuteNonQuery();
                MessageBox.Show("Congratulations, you got your daily bonus!\n" + "Streak: " + newUser.UserStreak + " Bonus: " + (5000 * newUser.UserStreak).ToString());
            }
            SqlCommand newLoginCmd = new SqlCommand("EXEC updateUserLastLogin @userID , @newLogin", sqlConnection);
            newLoginCmd.Parameters.AddWithValue("@userID", newUser.UserID);
            newLoginCmd.Parameters.AddWithValue("@newLogin", DateTime.Now);
            newLoginCmd.ExecuteNonQuery();
        }
        public void addWindow(string username)
        {
            SqlCommand command = new SqlCommand("EXEC getUser @username", sqlConnection);
            command.Parameters.AddWithValue("@username", username);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int userID = reader.IsDBNull(reader.GetOrdinal("user_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_id"));
                    string userName = reader.IsDBNull(reader.GetOrdinal("user_username")) ? "" : reader.GetString(reader.GetOrdinal("user_username"));
                    int currentFont = reader.IsDBNull(reader.GetOrdinal("user_currentFont")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_currentFont"));
                    int currentTitle = reader.IsDBNull(reader.GetOrdinal("user_currentTitle")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_currentTitle"));
                    int currentIcon = reader.IsDBNull(reader.GetOrdinal("user_currentIcon")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_currentIcon"));
                    int currentTable = reader.IsDBNull(reader.GetOrdinal("user_currentTable")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_currentTable"));
                    int chips = reader.IsDBNull(reader.GetOrdinal("user_chips")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_chips"));
                    int stack = reader.IsDBNull(reader.GetOrdinal("user_stack")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_stack"));
                    int streak = reader.IsDBNull(reader.GetOrdinal("user_streak")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_streak"));
                    int handsPlayed = reader.IsDBNull(reader.GetOrdinal("user_handsPlayed")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_handsPlayed"));
                    int level = reader.IsDBNull(reader.GetOrdinal("user_level")) ? 0 : reader.GetInt32(reader.GetOrdinal("user_level"));
                    DateTime lastLogin = reader.IsDBNull(reader.GetOrdinal("user_handsPlayed")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("user_lastLogin"));
                    User newUser = new User(userID, userName, currentFont, currentTitle, currentIcon, currentTable, chips, stack, streak, handsPlayed, level, lastLogin);
                    MenuWindow menuWindow = new MenuWindow(newUser, this);
                    reader.Close();
                    menuWindow.Show();
                    newUserLogin(newUser);
                    openedUsersWindows.Add(menuWindow);
                }
                else
                {
                    MessageBox.Show("The username is not valid.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void disconnectUser(MenuWindow window)
        {
            User player = window.Player();
            player.UserStatus = INACTIVE;
            internMutex.WaitOne();
            internTableUsers.Remove(window);
            internMutex.ReleaseMutex();
            juniorMutex.WaitOne();
            juniorTableUsers.Remove(window);
            juniorMutex.ReleaseMutex();
            seniorMutex.WaitOne();
            seniorTableUsers.Remove(window);
            seniorMutex.ReleaseMutex();
            SqlCommand updateChips = new SqlCommand("EXEC updateUserChips @userID , @userChips", sqlConnection);
            updateChips.Parameters.AddWithValue("@userID", player.UserID);
            updateChips.Parameters.AddWithValue("@userChips", player.UserChips + player.UserStack);
            updateChips.ExecuteNonQuery();
            player.UserChips += player.UserStack;
            SqlCommand resetStack = new SqlCommand("EXEC updateUserStack @userID , @userStack", sqlConnection);
            resetStack.Parameters.AddWithValue("@userID", player.UserID);
            resetStack.Parameters.AddWithValue("@userStack", 0);
            resetStack.ExecuteNonQuery();
            player.UserStack = EMPTY;
        }
        public bool joinInternTable(MenuWindow window)
        {
            if (isInternFull()) return false;
            User player = window.Player();
            if (player.UserChips < buyIn["Intern"]) return false; /// also return different values to differentiate full from no money
            SqlCommand updateChips = new SqlCommand("EXEC updateUserChips @userID , @userChips", sqlConnection);
            updateChips.Parameters.AddWithValue("@userID", player.UserID);
            updateChips.Parameters.AddWithValue("@userChips", player.UserChips - buyIn["Intern"]);
            updateChips.ExecuteNonQuery();
            SqlCommand resetStack = new SqlCommand("EXEC updateUserStack @userID , @userStack", sqlConnection);
            resetStack.Parameters.AddWithValue("@userID", player.UserID);
            resetStack.Parameters.AddWithValue("@userStack", buyIn["Intern"]);
            resetStack.ExecuteNonQuery();
            player.UserChips -= buyIn["Intern"];
            player.UserStack = buyIn["Intern"];
            player.UserStatus = WAITING;
            internMutex.WaitOne();
            internTableUsers.Add(window);
            internMutex.ReleaseMutex();
            return true;
        }

        public bool joinJuniorTable(MenuWindow window)
        {
            if (isJuniorFull()) return false;
            User player = window.Player();
            if (player.UserChips < buyIn["Junior"]) return false;
            SqlCommand updateChips = new SqlCommand("EXEC updateUserChips @userID , @userChips", sqlConnection);
            updateChips.Parameters.AddWithValue("@userID", player.UserID);
            updateChips.Parameters.AddWithValue("@userChips", player.UserChips - buyIn["Junior"]);
            updateChips.ExecuteNonQuery();
            SqlCommand resetStack = new SqlCommand("EXEC updateUserStack @userID , @userStack", sqlConnection);
            resetStack.Parameters.AddWithValue("@userID", player.UserID);
            resetStack.Parameters.AddWithValue("@userStack", buyIn["Junior"]);
            resetStack.ExecuteNonQuery();
            player.UserChips -= buyIn["Junior"];
            player.UserStack = buyIn["Junior"];
            player.UserStatus = WAITING;
            juniorMutex.WaitOne();
            juniorTableUsers.Add(window);
            juniorMutex.ReleaseMutex();
            return true;
        }

        public bool joinSeniorTable(MenuWindow window)
        {
            if (isSeniorFull()) return false;
            User player = window.Player();
            if (player.UserChips < buyIn["Senior"]) return false;
            SqlCommand updateChips = new SqlCommand("EXEC updateUserChips @userID , @userChips", sqlConnection);
            updateChips.Parameters.AddWithValue("@userID", player.UserID);
            updateChips.Parameters.AddWithValue("@userChips", player.UserChips - buyIn["Senior"]);
            updateChips.ExecuteNonQuery();
            SqlCommand resetStack = new SqlCommand("EXEC updateUserStack @userID , @userStack", sqlConnection);
            resetStack.Parameters.AddWithValue("@userID", player.UserID);
            resetStack.Parameters.AddWithValue("@userStack", buyIn["Senior"]);
            resetStack.ExecuteNonQuery();
            player.UserChips -= buyIn["Senior"];
            player.UserStack = buyIn["Senior"];
            player.UserStatus = WAITING;
            seniorMutex.WaitOne();
            seniorTableUsers.Add(window);
            seniorMutex.ReleaseMutex();
            return true;
        }

        public bool isInternFull()
        {
            return internTableUsers.Count == FULL;
        }

        public bool isJuniorFull()
        {
            return juniorTableUsers.Count == FULL;
        }

        public bool isSeniorFull()
        {
            return seniorTableUsers.Count == FULL;
        }

        public bool isInternEmpty()
        {
            return internTableUsers.Count == EMPTY;
        }

        public bool isJuniorEmpty()
        {
            return juniorTableUsers.Count == EMPTY;
        }

        public bool isSeniorEmpty()
        {
            return seniorTableUsers.Count == EMPTY;
        }

        public int occupiedIntern()
        {
            return internTableUsers.Count;
        }

        public int occupiedJunior()
        {
            return juniorTableUsers.Count;
        }

        public int occupiedSenior()
        {
            return seniorTableUsers.Count;
        }

        public void endGames()
        {
            ended = true;
        }
    }
}
