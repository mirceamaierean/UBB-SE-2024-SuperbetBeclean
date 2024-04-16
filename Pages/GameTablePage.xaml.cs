using SuperbetBeclean.Services;
using SuperbetBeclean.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
    /// Interaction logic for GameTablePage.xaml
    /// </summary>
    public partial class GameTablePage : Page
    {
        private MenuWindow _mainWindow;
        private Frame _mainFrame;
        private MainService _service;
        int timer = 0;
        int playerBet = 0;
        int tableBet = 0;
        string action = "";

        public GameTablePage(Frame mainFrame, MenuWindow mainWindow, MainService service)
        {
            InitializeComponent();
            Loaded += GameTablePage_Loaded;
            _mainWindow = mainWindow;
            _service = service;
            _mainFrame = mainFrame;
            PlayerNameTextBox.Text = _mainWindow.userName();
            PlayerLvlTextBox.Text = "Level: " + _mainWindow.userLevel().ToString();
            PlayerChipsTextBox.Text = "Chips: " + _mainWindow.userChips().ToString();
        }

        private void GameTablePage_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window != null)
            {
                window.Width = 920;
                window.Height = 720;
            }
        }
        
        private void QuitBttn_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.NavigationService.GoBack();
            _mainWindow.disconnectUser(sender, e);
        }

        public void endTimer()
        {
            PlayerTimer.Text = "";
            timer = -1;
            PlayerTimer.Foreground = Brushes.White;
        }

        public void endTurn()
        {
            tableBet = 0;
            Application.Current.Dispatcher.Invoke(() => {
                PotValue.Content = tableBet.ToString();
            });
        }

        public void resetTimer()
        {
            timer = 15;
            action = "";
            playerBet = 0;
            PlayerTimer.Text = "Time: " + timer.ToString();
            PlayerTimer.Foreground = Brushes.White;
        }
        public void decrementTimer()
        {
            timer--;
            if (timer == 5) 
                PlayerTimer.Foreground = Brushes.Red;
            PlayerTimer.Text = "Time: " + timer.ToString();
        }

        public void resetCards()
        {
            for (int i=1;i<=8;i++)
            {
                for (int j=1;j<=2;j++)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        Image playerCard = FindName("Player" + i + "Card" + j) as Image;
                        playerCard.Visibility = Visibility.Hidden;
                    });
                }
            }
            
        }

        public void addCard(bool visible, int player, int card, string cardValue)
        {
            Application.Current.Dispatcher.Invoke(() => {
                Image playerCard = FindName("Player" + player + "Card" + card) as Image;
                if (visible == false)
                {
                    Uri uri = new Uri("/assets/cards/downCard.jpg", UriKind.Relative);
                    playerCard.Source = new BitmapImage(uri);
                }
                else
                {
                    Uri uri = new Uri("/assets/cards/" + cardValue + ".png", UriKind.Relative);
                    playerCard.Source = new BitmapImage(uri);
                }
                playerCard.Visibility = Visibility.Visible;
            });
        }

        async public Task < int > runTimer()
        {
            Console.WriteLine("hey i start timer!");
            Application.Current.Dispatcher.Invoke(() => {
                resetTimer();
            });
            while (timer != 0)
            {
                if (action != "") break;
                await Task.Delay(1000);
                Application.Current.Dispatcher.Invoke(() => {
                    decrementTimer();
                });
                Console.WriteLine(timer.ToString());
            }
            Application.Current.Dispatcher.Invoke(() => { 
                endTimer(); 
            });
            Application.Current.Dispatcher.Invoke(() => {
                PotValue.Content = tableBet.ToString();
            });
            return playerBet;
        }

        private void CallBtn_Click(object sender, RoutedEventArgs e)
        {
            action = "Call";
            playerBet = tableBet;
        }

        private void RaiseBttn_Click(object sender, RoutedEventArgs e)
        {
            action = "Raise";
            tableBet = Int32.Parse(BetInput.Text);
            playerBet = tableBet;
        }

        public void updatePot(int pot)
        {
            tableBet = pot;
            Application.Current.Dispatcher.Invoke(() => {
                PotValue.Content = tableBet.ToString();
            });
        }
        private void ChallengesBttn_Click(object sender, RoutedEventArgs e)
        {
            ChallengesWindow challengesWindow= new ChallengesWindow();
            challengesWindow.Show();
        }

        private void MsgBttn_Click(object sender, RoutedEventArgs e)
        {
            ChatWindow chatWindow = new ChatWindow();
            chatWindow.Show();
        }
    }
}
