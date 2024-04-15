using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using SuperbetBeclean.Model;
using SuperbetBeclean.Windows;

namespace SuperbetBeclean.Services
{
    public class Service
    {
        private static Mutex internMutex, juniorMutex, seniorMutex;
        private List<Window> openedUsersWindows;
        private List<User> activeUsers;
        private SqlConnection sqlConnection;
        const int FULL = 8;
        bool ended = false;
        const int EMPTY = 0;
        private Dictionary<string, int> buyIn, smallBlind, bigBlind;
        private List<User> internTableUsers;
        private List<User> juniorTableUsers;
        private List<User> seniorTableUsers;
        string connectionString;
        Thread internThread, juniorThread, seniorThread;

        public Service()
        {
            connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
            openedUsersWindows = new List<Window>();
            internTableUsers = new List<User>();
            juniorTableUsers = new List<User>();
            seniorTableUsers = new List<User>();
            activeUsers = new List<User>();
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            buyIn = new Dictionary<string, int>();
            smallBlind = new Dictionary<string, int>();
            bigBlind = new Dictionary<string, int>();
            buyIn["Intern"] = 5000; buyIn["Junior"] = 50000; buyIn["Senior"] = 500000;
            smallBlind["Intern"] = 50; smallBlind["Junior"] = 500; smallBlind["Senior"] = 5000;
            bigBlind["Intern"] = 100; bigBlind["Junior"] = 1000; bigBlind["Senior"] = 10000;
            internThread = new Thread(new ThreadStart(runInternTable));
            juniorThread = new Thread(new ThreadStart(runJuniorTable));
            seniorThread = new Thread(new ThreadStart(runSeniorTable));
            internMutex = new Mutex(); juniorMutex = new Mutex(); seniorMutex = new Mutex();
            internThread.Start(); juniorThread.Start(); seniorThread.Start();
        }


        private void runInternTable()
        {
            while (true)
            {
                if (ended) break;
                internMutex.WaitOne();
                Queue<User> activePlayers = new Queue<User>(internTableUsers);
                internMutex.ReleaseMutex();
                if (isInternEmpty())
                {
                    Thread.Sleep(3000);
                    Console.WriteLine("we have no players :(");
                    continue;
                }
                // Console.WriteLine("we have a player!!!");
                // Thread.Sleep(3000);
                /// TO DO : play a round
                /// 1. deal the cards to the players
                /// 2. play the flop (first 3 cards)
                /// 3. play the turn (fourth card)
                /// 4. play the river (fifth card)
                /// 5. showdown and pot split
                /// 6. reset everything
                /// at the end of round, add players standing in a queue to the table 
            }
        }

        private void runJuniorTable()
        {
            while (true)
            {
                if (ended) break;
                juniorMutex.WaitOne();
                Queue<User> activePlayers = new Queue<User>(juniorTableUsers);
                juniorMutex.ReleaseMutex();
                if (isJuniorEmpty())
                {
                    Thread.Sleep(3000);
                    continue;
                }
                /// TO DO : play a round
                /// 1. deal the cards to the players
                /// 2. play the flop (first 3 cards)
                /// 3. play the turn (fourth card)
                /// 4. play the river (fifth card)
                /// 5. showdown and pot split
                /// 6. reset everything
            }
        }

        private void runSeniorTable()
        {
            while (true)
            {
                if (ended) break;
                seniorMutex.WaitOne();
                Queue<User> activePlayers = new Queue<User>(seniorTableUsers);
                seniorMutex.ReleaseMutex();
                if (isSeniorEmpty())
                {
                    Thread.Sleep(3000);
                    continue;
                }
                /// TO DO : play a round
                /// at the end of round, add players standing in a queue to the table 
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
                    activeUsers.Add(newUser);
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
        public void disconnectUser(User u)
        {
            internMutex.WaitOne();
            internTableUsers.Remove(u);
            internMutex.ReleaseMutex();
            juniorMutex.WaitOne();
            juniorTableUsers.Remove(u);
            juniorMutex.ReleaseMutex();
            seniorMutex.WaitOne();
            seniorTableUsers.Remove(u);
            seniorMutex.ReleaseMutex();
        }
        public bool joinInternTable(User u)
        {
            if (isInternFull()) return false;
            if (u.UserChips < buyIn["Intern"]) return false; /// also return different values to differentiate full from no money
            u.UserChips -= buyIn["Intern"];
            u.UserStack = buyIn["Intern"];
            /// maybe the database should also be updated with these changes, or just locally store all the data until the user quits the app
            internMutex.WaitOne();
            internTableUsers.Add(u);
            internMutex.ReleaseMutex();
            return true;
        }

        public bool joinJuniorTable(User u)
        {
            if (isJuniorFull()) return false;
            if (u.UserChips < buyIn["Junior"]) return false;
            u.UserChips -= buyIn["Junior"];
            u.UserStack = buyIn["Junior"];
            juniorMutex.WaitOne();
            juniorTableUsers.Add(u);
            juniorMutex.ReleaseMutex();
            return true;
        }

        public bool joinSeniorTable(User u)
        {
            if (isSeniorFull()) return false;
            if (u.UserChips < buyIn["Senior"]) return false;
            u.UserChips -= buyIn["Senior"];
            u.UserStack = buyIn["Senior"];
            seniorMutex.WaitOne();
            seniorTableUsers.Add(u);
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
            internThread.Join();
            juniorThread.Join();
            seniorThread.Join();
        }
    }
}
