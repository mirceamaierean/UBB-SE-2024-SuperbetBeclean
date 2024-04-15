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
        private Service _service;
        public MainMenu(Frame mainFrame, MenuWindow mainWindow, Service serv, User user)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _menuWindow = mainWindow;
            _service = serv;
            _user = user;
        }

        private void onClickRulesButton(object sender, RoutedEventArgs e)
        {
            RulesWindow rulesWindow = new RulesWindow();
            rulesWindow.Show();
        }

        private void onClickPlayButton(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LobbyPage(_mainFrame, _menuWindow, _service, _user));
        }

        private void onClickQuitButton(object sender, RoutedEventArgs e)
        {
            _menuWindow.Close();
        }

    }
}
