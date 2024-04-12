using SuperbetBeclean.Pages;
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
using SuperbetBeclean.Services;
using SuperbetBeclean.Model;

namespace SuperbetBeclean.Windows
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private User user;
        public MenuWindow(User u)
        {
            InitializeComponent();
            this.user = u;
            MenuFrame.Navigate(new MainMenu(MenuFrame, this));
            this.Title = user.UserName;
        }
    }
}
