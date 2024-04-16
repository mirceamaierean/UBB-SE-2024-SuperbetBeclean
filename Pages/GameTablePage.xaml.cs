using SuperbetBeclean.Services;
using SuperbetBeclean.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<ChatWindow> chatWindows;
        string tableType = "intern example";
        ChatWindow _chatWindow;
        private Dictionary<MenuWindow, ChatWindow> menuWindowChatWindowMap = new Dictionary<MenuWindow, ChatWindow>();
        private ChatService _chatService;
        private List<ChatWindow> openChatWindows = new List<ChatWindow>();
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
           
            _chatService = new ChatService();


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
            _chatService.newChat(_mainWindow);
            
        }
        
    }
}
