using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using SuperbetBeclean.Model;
using SuperbetBeclean.Windows;

namespace SuperbetBeclean.Services
{
    public class MainService
    {
        private List < Window > openedUsersWindows;
        private SqlConnection sqlConnection;
        string connectionString;
        public MainService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
            openedUsersWindows = new List < Window >();
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
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
                    MenuWindow menuWindow = new MenuWindow(new User(userID, userName, currentFont, currentTitle, currentIcon, currentTable, chips, stack, streak, handsPlayed, level, lastLogin));
                    menuWindow.Show();
                    openedUsersWindows.Add(menuWindow);
                }
                else
                {
                    MessageBox.Show("The username is not valid.");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
