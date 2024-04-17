using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SuperbetBeclean.Services; // Assuming you have a service for retrieving requests

namespace SuperbetBeclean
{
    /// <summary>
    /// Interaction logic for RequestsWindow.xaml
    /// </summary>
    public partial class RequestsWindow : Window
    {
        private DBService _dbService;
        string _currentUserName;
        List<string> requests;
        public RequestsWindow(string currentUserName)
        {
            InitializeComponent();
            _dbService = new DBService(); // Initialize the database service
            _currentUserName = currentUserName;
            // Call a method to load and display requests
            LoadRequests();
            chipsInRequestPage.Text = _dbService.GetChipsByUserId(_dbService.GetUserIdByUserName(_currentUserName)).ToString();
        }

        private void LoadRequests()
        {
            requests = _dbService.GetAllRequestsByToUserID(_dbService.GetUserIdByUserName(_currentUserName)); // Get requests from the database

            // Create and add request items dynamically
            foreach (string requestInfo in requests)
            {
                Border requestBorder = new Border();
                requestBorder.CornerRadius = new CornerRadius(5);
                requestBorder.Background = Brushes.White;
                requestBorder.Margin = new Thickness(5);

                StackPanel requestPanel = new StackPanel();
                requestPanel.Orientation = Orientation.Horizontal;
                requestPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

                TextBlock requestTextBlock = new TextBlock();
                requestTextBlock.Text = requestInfo;
                requestTextBlock.Margin = new Thickness(10);
                requestTextBlock.VerticalAlignment = VerticalAlignment.Center;

                

                requestPanel.Children.Add(requestTextBlock);
            

                requestBorder.Child = requestPanel;

                RequestsStackPanel.Children.Add(requestBorder);
                

            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle accept button click event
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle decline button click event
        }
        //Accept all
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<int, int>> requests = _dbService.GetAllRequestsByToUserIDSimplified(_dbService.GetUserIdByUserName(_currentUserName));

            foreach (Tuple<int, int> request in requests)
            {
                int fromUserID = request.Item1;
                int toUserID = request.Item2;
                int numberChips = _dbService.GetChipsByUserId(fromUserID)+3000;
                _dbService.UpdateUserChips(fromUserID, numberChips);
               
            }
            _dbService.DeleteRequestsByUserId(_dbService.GetUserIdByUserName(_currentUserName));
            LoadRequests();
        }
        //Decline all
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _dbService.DeleteRequestsByUserId(_dbService.GetUserIdByUserName(_currentUserName));
            LoadRequests();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {   if(_dbService.GetChipsByUserId(_dbService.GetUserIdByUserName(_currentUserName)) == 0) { 
            try {
                string playerToSend = playerToSendRequest.Text;
                    if (_dbService.GetUserIdByUserName(playerToSend) == -1)
                    {
                        MessageBox.Show("Can't find the specified player.");
                        return;
                    }
                int firstPlayerID = _dbService.GetUserIdByUserName(_currentUserName);
                int secondPlayerID = _dbService.GetUserIdByUserName(playerToSend);
                _dbService.CreateRequest(firstPlayerID, secondPlayerID);

            }
            catch
            {
                MessageBox.Show("Can't send multiple request on the same day");
            }
            }
            else
            {
                MessageBox.Show("You must have 0 chips to be able to send requests!");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
