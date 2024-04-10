using System.Windows.Controls;

namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for LobbyPage.xaml
    /// </summary>
    public partial class LobbyPage : Page
    {
        private Frame _mainFrame;
        public LobbyPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void buttonLobbyBack(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MainMenu(_mainFrame));
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
    }
}
