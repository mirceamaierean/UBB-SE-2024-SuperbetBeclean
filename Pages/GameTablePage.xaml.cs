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
        private Service _service;
        private Frame _mainFrame;
        int timer = 0;

        public GameTablePage(Frame mainFrame, MenuWindow mainWindow, Service service)
        {
            InitializeComponent();
            Loaded += GameTablePage_Loaded;
            _mainFrame = mainFrame;
            _mainWindow = mainWindow;
            _service = service;
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

        public void resetTimer()
        {
            timer = 15;
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

        async public Task runTimer()
        {
            Console.WriteLine("hey i start timer!");
            Application.Current.Dispatcher.Invoke(() => {
                resetTimer();
            });
            while (timer != 0)
            {
                await Task.Delay(1000);
                Application.Current.Dispatcher.Invoke(() => {
                    decrementTimer();
                });
                Console.WriteLine(timer.ToString());
            }
            Application.Current.Dispatcher.Invoke(() => { 
                endTimer(); 
            });
        }
    }
}
