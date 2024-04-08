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

namespace SuperbetBeclean
{
    /// <summary>
    /// Interaction logic for WelcomeToPokerWindow.xaml
    /// </summary>
    public partial class WelcomeToPokerWindow : Window
    {
        public WelcomeToPokerWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new WelcomeToPokerMainPage());   
        }

        private void switchOnClickToRulesPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RulesPageWindow());
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void MainFrame_Navigated_1(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
