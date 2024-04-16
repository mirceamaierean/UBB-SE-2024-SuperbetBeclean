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
        private MainService service;
        public GameTablePage internPage, juniorPage, seniorPage;

        public MenuWindow(User u, MainService serv)
        {
            InitializeComponent();
            this.service = serv;
            this.user = u;
            this.Title = user.UserName;
            MenuFrame.Navigate(new MainMenu(MenuFrame, this, serv, user));
            internPage = new GameTablePage(MenuFrame, this, service,"intern");
            juniorPage = new GameTablePage(MenuFrame, this, service,"junior");
            seniorPage = new GameTablePage(MenuFrame, this, service,"senior");
            Closed += disconnectUser;
        }
        public void disconnectUser(object sender, System.EventArgs e)
        {
            service.disconnectUser(this);
        }

        async public Task < int > startTime(string table)
        {
            int bet = 0;
            if (table == "intern")
                bet = await internPage.runTimer();
            if (table == "junior")
                bet = await juniorPage.runTimer();
            if (table == "senior")
                bet = await seniorPage.runTimer();
            return bet;
        }

        public void notify(string table, int currentPot)
        {
            if (table == "intern")
            {
                internPage.updatePot(currentPot);
            }
            if (table == "junior")
            {
                juniorPage.updatePot(currentPot);
            }
            if (table == "senior")
            {
                seniorPage.updatePot(currentPot);
            }
        }

        public void endTurn(string table)
        {
            if (table == "intern")
                internPage.endTurn();
            if (table == "junior")
                juniorPage.endTurn();
            if (table == "senior")
                seniorPage.endTurn();
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
