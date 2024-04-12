using SuperbetBeclean.Windows;
using System.Windows;
using System.Windows.Controls;

namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for LobbyPage.xaml
    /// </summary>
    public partial class LobbyPage : Page
    {
        private Frame _mainFrame;
        private MenuWindow _mainWindow;
        ///PlayerNameTextBoxPlayerLvlTextBoxChipsLevelTextBox
        public LobbyPage(Frame mainFrame, MenuWindow menuWindow)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _mainWindow = menuWindow;
            PlayerNameTextBox.Text = menuWindow.userName();
            PlayerLevelTextBox.Text = "Level: " + menuWindow.userLevel().ToString();
            PlayerChipsTextBox.Text = "Chips: " + menuWindow.userChips().ToString();
        }

        private void buttonLobbyBack(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainFrame.NavigationService.GoBack();
        }

        private void onClickLeaderboardButton(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LeaderboardPage(_mainFrame));
        }


        private void onClickInternButton(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainFrame.Navigate(new GameTablePage(_mainFrame));
        }

        private void onClickJuniorBttn(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainFrame.Navigate(new GameTablePage(_mainFrame));
        }

        private void onClickSeniorButton(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainFrame.Navigate(new GameTablePage(_mainFrame));
        }
        private void PlayerIconImg_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mainFrame.Navigate(new ProfilePage(_mainFrame));
        }
    }
}