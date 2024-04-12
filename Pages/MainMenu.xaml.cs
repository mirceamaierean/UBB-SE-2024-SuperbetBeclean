using System.Windows;
using System.Windows.Controls;
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

        public MainMenu(Frame mainFrame, MenuWindow mainWindow)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _menuWindow = mainWindow;
        }

        private void onClickRulesButton(object sender, RoutedEventArgs e)
        {
            RulesWindow rulesWindow = new RulesWindow();
            rulesWindow.Show();
        }

        private void onClickPlayButton(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LobbyPage(_mainFrame, _menuWindow));
        }

        private void onClickQuitButton(object sender, RoutedEventArgs e)
        {
            _menuWindow.Close();
        }

    }
}
