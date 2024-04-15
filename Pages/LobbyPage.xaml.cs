using SuperbetBeclean.Model;
using SuperbetBeclean.Services;
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
        private Service _service;
        private User _user;
        public LobbyPage(Frame mainFrame, MenuWindow menuWindow, Service service, User u)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _mainWindow = menuWindow;
            _service = service;
            _user = u;
            PlayerNameTextBox.Text = menuWindow.userName();
            PlayerLevelTextBox.Text = "Level: " + menuWindow.userLevel().ToString();
            PlayerChipsTextBox.Text = "Chips: " + menuWindow.userChips().ToString();
            InternPlayerCount.Text = _service.occupiedIntern().ToString() + "/8";
            JuniorPlayerCount.Text = _service.occupiedJunior().ToString() + "/8";
            SeniorPlayerCount.Text = _service.occupiedSenior().ToString() + "/8";
        }

        private void buttonLobbyBack(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainFrame.NavigationService.GoBack();
        }

        private void onClickLeaderboardButton(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LeaderboardPage(_mainFrame));
        }

        private void onShopButtonClick(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new ShopPage(_mainFrame));
        }

        private void onClickInternButton(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_service.joinInternTable(_mainWindow))
                _mainFrame.Navigate(_mainWindow.internPage);
            else
                MessageBox.Show("Sorry, this table is full.");
        }

        private void onClickJuniorBttn(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_service.joinJuniorTable(_mainWindow))
                _mainFrame.Navigate(_mainWindow.juniorPage);
            else
                MessageBox.Show("Sorry, this table is full.");
        }

        private void onClickSeniorButton(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_service.joinSeniorTable(_mainWindow))
                _mainFrame.Navigate(_mainWindow.seniorPage);
            else
                MessageBox.Show("Sorry, this table is full.");
        }
        private void PlayerIconImg_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _mainFrame.Navigate(new ProfilePage(_mainFrame, _mainWindow));
        }
    }
}