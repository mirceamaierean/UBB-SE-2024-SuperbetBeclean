using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using SuperbetBeclean.Model;
using SuperbetBeclean.Windows;

namespace SuperbetBeclean.Services
{
    public class MainSubject
    {
        private List<Window> openedUsersWindows;
        private List<User> activeUsers;
        private SqlConnection sqlConnection;
        string connectionString;
        public MainSubject()
        {
            connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
            openedUsersWindows = new List<Window>();
            activeUsers = new List<User>();
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
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
                    MenuWindow menuWindow = new MenuWindow(newUser, new GameService());
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
    }
}
