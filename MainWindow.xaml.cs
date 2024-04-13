using System.Windows;
using SuperbetBeclean.Pages;
using SuperbetBeclean.Services;

namespace SuperbetBeclean
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainSubject subject;
        public MainWindow()
        {
            InitializeComponent();
            subject = new MainSubject();
            MainFrame.Navigate(new LoginPage(MainFrame, this));
            Title = "Superbet Beclean - Poker";
        }
        public void openNewWindow(string username)
        {
            subject.addWindow(username);
        }
    }
}
