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
        public LobbyPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }


        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void onShopButtonClick(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new ShopPage(_mainFrame));
        }
    }
}
