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
        Dictionary < string , GameTablePage > gamePages;

        public MenuWindow(User u, MainService serv)
        {
            InitializeComponent();
            this.service = serv;
            this.user = u;
            this.Title = user.UserName;
            MenuFrame.Navigate(new MainMenu(MenuFrame, this, serv, user));
            gamePages = new Dictionary<string, GameTablePage> ();
            gamePages.Add("intern", new GameTablePage(MenuFrame, this, service));
            gamePages.Add("junior", new GameTablePage(MenuFrame, this, service));
            gamePages.Add("senior", new GameTablePage(MenuFrame, this, service));
            Closed += disconnectUser;
        }
        public void disconnectUser(object sender, System.EventArgs e)
        {
            service.disconnectUser(this);
        }

        async public Task < int > startTime(string table, int minBet, int maxBet)
        {
            int bet = 0;
            bet = await gamePages[table].runTimer(minBet, maxBet);
            return bet;
        }

        public void resetCards(string table)
        {
            gamePages[table].resetCards();
        }

        public void notifyUserCard(string table, User u, int card, string cardValue)
        {
            gamePages[table].addUserCard(u == user, u.UserTablePlace, card, cardValue);
        }

        public void notifyTableCard(string table, int card, string cardValue)
        {
            gamePages[table].addTableCard(card, cardValue);
        }

        public void showCards(string table, User player)
        {
            gamePages[table].showCards(player);
        }
        public void notify(string table, User player, int tablePot, int tableBet)
        {
            gamePages[table].updatePlayerMoney(player);
            gamePages[table].updatePot(tablePot, tableBet);
        }
        public void foldPlayer(string table, User player)
        {
            gamePages[table].playerFold(player);
        }
        public void showPlayer(string table, User player)
        {
            gamePages[table].showPlayer(player);
        }

        public void hidePlayer(string table, User player)
        {
            gamePages[table].hidePlayer(player);
        }
        public void endTurn(string table, User player)
        {
            gamePages[table].endTurn(player);
        }
        public void startRound(string table, User player)
        {
            gamePages[table].startRound(player);
        }
        public void resetPot(string table)
        {
            gamePages[table].resetPot();
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

        public GameTablePage internPage()
        {
            return gamePages["intern"];
        }
        public GameTablePage juniorPage()
        {
            return gamePages["junior"];
        }
        public GameTablePage seniorPage()
        {
            return gamePages["senior"];
        }
    }
}
