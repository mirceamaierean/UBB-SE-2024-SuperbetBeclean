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
        private MainService subject;
        public MainWindow()
        {
            InitializeComponent();
            subject = new MainService();
            MainFrame.Navigate(new LoginPage(MainFrame, this));
        }

        public void openNewWindow(string username)
        {
            subject.addWindow(username);
        }
    }
}
