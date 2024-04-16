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

namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        private Frame _mainFrame;
        public ShopPage(Frame mainFrame)
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            _mainFrame = mainFrame;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.NavigationService.GoBack();
        }

    }
}
