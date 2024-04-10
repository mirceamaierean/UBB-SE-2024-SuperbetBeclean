using System.Windows;
using System.Windows.Controls;

namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for LeaderboardPage.xaml
    /// </summary>
    public partial class LeaderboardPage : Page
    {
        private Frame _mainFrame;
        public LeaderboardPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void onClickLeaderboardBack(object sender, RoutedEventArgs e)
        {
            _mainFrame.NavigationService.GoBack();
        }
    }
}
