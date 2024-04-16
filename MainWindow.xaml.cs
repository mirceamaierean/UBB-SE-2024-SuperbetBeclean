using System.Windows;
using SuperbetBeclean.Model;
using SuperbetBeclean.Pages;
using SuperbetBeclean.Services;

namespace SuperbetBeclean
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainService service;
        public MainWindow()
        {
            InitializeComponent();
            service = new MainService();
            MainFrame.Navigate(new LoginPage(MainFrame, this));
            Title = "Superbet Beclean - Poker";
            Closed += endGames;
        }
        public void openNewWindow(string username)
        {
            service.addWindow(username);
        }

        private void endGames(object sender, System.EventArgs e)
        {
            service.endGames();
        }
    }
}
