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

        public MainMenu(Frame mainFrame, string username = "")
        {
            InitializeComponent();
            _mainFrame = mainFrame;
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
            System.Environment.Exit(0);
        }

    }
}
