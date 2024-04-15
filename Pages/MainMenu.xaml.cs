using System.Windows;
using System.Windows.Controls;
using SuperbetBeclean.Model;
using SuperbetBeclean.Services;
using SuperbetBeclean.Windows;

namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private Frame _mainFrame;
        private MenuWindow _menuWindow;
        private User _user;
        private GameService _gameService;
        public MainMenu(Frame mainFrame, MenuWindow mainWindow, GameService serv, User user)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _menuWindow = mainWindow;
            _gameService = serv;
            _user = user;
        }

        private void onClickRulesButton(object sender, RoutedEventArgs e)
        {
            RulesWindow rulesWindow = new RulesWindow();
            rulesWindow.Show();
        }

        private void onClickPlayButton(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LobbyPage(_mainFrame, _menuWindow, _gameService, _user));
        }

        private void onClickQuitButton(object sender, RoutedEventArgs e)
        {
            _menuWindow.Close();
        }

    }
}
