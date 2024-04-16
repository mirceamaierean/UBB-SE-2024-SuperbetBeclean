using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SuperbetBeclean.Models;
using SuperbetBeclean.Windows;

namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        private Frame _mainFrame;
        public ShopPage(Frame mainFrame, MenuWindow _menuWindow)
        {
            InitializeComponent();
            DataContext = new MainViewModel(_menuWindow.userChips());
            _mainFrame = mainFrame;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.NavigationService.GoBack();
        }

        public static readonly DependencyProperty BalanceProperty = DependencyProperty.Register(
                       "Balance", typeof(List<ShopItem>), typeof(ShopPage), new PropertyMetadata(default(List<ShopItem>)));

        public int Balance
        {
            get { return (int)GetValue(BalanceProperty); }
            set { SetValue(BalanceProperty, value); }
        }
    }
}
