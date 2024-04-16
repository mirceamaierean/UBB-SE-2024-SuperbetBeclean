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
    public class MainService
    {
        private static Mutex internMutex, juniorMutex, seniorMutex;
        private List<MenuWindow> openedUsersWindows;
        private SqlConnection sqlConnection;
        const int FULL = 8, EMPTY = 0;
        bool ended = false;
        const int INACTIVE = 0, WAITING = 1, PLAYING = 2;

        private TableService internTable, juniorTable, seniorTable;
        string connectionString;
        // Task internTask, juniorTask, seniorTask;

        public MainService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            openedUsersWindows = new List<MenuWindow>();
            internTable = new TableService(5000, 50, 100, "intern");
            juniorTable = new TableService(50000, 500, 1000, "junior");
            seniorTable = new TableService(500000, 5000, 10000, "senior");
        }

        public int occupiedIntern()
        {
            return internTable.occupied();
        }

        public int occupiedJunior()
        {
            return juniorTable.occupied();
        }

        public int occupiedSenior()
        {
            return seniorTable.occupied();
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

            internTable.disconnectUser(window);
            juniorTable.disconnectUser(window);
            seniorTable.disconnectUser(window);

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
            return internTable.joinTable(window, ref sqlConnection);
        }

        public bool joinJuniorTable(MenuWindow window)
        {
            return juniorTable.joinTable(window, ref sqlConnection);
        }

        public bool joinSeniorTable(MenuWindow window)
        {
            return seniorTable.joinTable(window, ref sqlConnection);
        }
        public void endGames()
        {
            ended = true;
        }
    }
}
