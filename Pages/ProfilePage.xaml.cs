using SuperbetBeclean.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private Frame _mainFrame;
        private MenuWindow _mainWindow;
        public ProfilePage(Frame mainFrame, MenuWindow mainWindow)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _mainWindow = mainWindow;
            profilePageUsernameTextBlock.Text = mainWindow.userName();
            profilePageChipsTextBlock.Text = mainWindow.userChips().ToString();
            profilePageDailyStreakTextBlock.Text = mainWindow.userStreak().ToString();
            profilePageLevelTextBlock.Text = mainWindow.userLevel().ToString() + ": ";
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mainFrame.NavigationService.GoBack();
        }
    }
}
