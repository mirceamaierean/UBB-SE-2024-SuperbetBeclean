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
        private Window _mainWindow;

        public MainMenu(Frame mainFrame, Window mainWindow)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _mainWindow = mainWindow;
        }

        private void onClickRulesButton(object sender, RoutedEventArgs e)
        {
            RulesWindow rulesWindow = new RulesWindow();
            rulesWindow.Show();
        }

        private void onClickPlayButton(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LobbyPage(_mainFrame));
        }

        private void onClickQuitButton(object sender, RoutedEventArgs e)
        {
            _mainWindow.Close();
        }

    }
}
