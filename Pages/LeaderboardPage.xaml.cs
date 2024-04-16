using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for LeaderboardPage.xaml
    /// </summary>
    public partial class LeaderboardPage : Page
    {
        private Frame _mainFrame;
        private List<string> _leaderBoardVector;

        public LeaderboardPage(Frame mainFrame, List<string> leaderBoardVector)
        {
            InitializeComponent();
            _leaderBoardVector = leaderBoardVector;
            _mainFrame = mainFrame;

            // Set the ItemsSource of the ListView to leaderBoardVector
            listViewLeaderboard.ItemsSource = _leaderBoardVector;

            // Subscribe to the Loaded event of the ListView
            listViewLeaderboard.Loaded += ListViewLeaderboard_Loaded;
        }

        private void ListViewLeaderboard_Loaded(object sender, RoutedEventArgs e)
        {
            // Set font color for the first three items
            for (int i = 0; i < 3 && i < listViewLeaderboard.Items.Count; i++)
            {
                var item = listViewLeaderboard.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                if (item != null && i ==0)
                {
                    item.Foreground = Brushes.Gold; // Change font color for the first place
                }
                if (item != null && i == 1)
                {
                    item.Foreground = Brushes.Silver; // Change font color for the second place
                }
                if (item != null && i == 2)
                {
                    item.Foreground = Brushes.BlanchedAlmond; // Change font color for the third place
                }
            }
        }

        private void onClickLeaderboardBack(object sender, RoutedEventArgs e)
        {
            _mainFrame.NavigationService.GoBack();
        }
    }
}
