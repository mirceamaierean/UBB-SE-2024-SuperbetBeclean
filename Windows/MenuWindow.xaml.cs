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
using System.Threading;

namespace SuperbetBeclean.Windows
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private User user;
        private Service service;
        public GameTablePage gamePage;

        public MenuWindow(User u, Service serv)
        {
            InitializeComponent();
            this.service = serv;
            this.user = u;
            this.Title = user.UserName;
            MenuFrame.Navigate(new MainMenu(MenuFrame, this, serv, user));
            gamePage = new GameTablePage(MenuFrame, this, service);
            Closed += disconnectUser;
        }
        public void disconnectUser(object sender, System.EventArgs e)
        {
            service.disconnectUser(this);
        }

        async public Task startTime()
        {
            await gamePage.runTimer();
        }

        public User Player()
        {
            return user;
        }

        public string userName()
        {
            return user.UserName;
        }

        public int userLevel()
        {
            return user.UserLevel;
        }

        public int userChips()
        {
            return user.UserChips;
        }

        public int userStreak()
        {
            return user.UserStreak;
        }
    }
}
