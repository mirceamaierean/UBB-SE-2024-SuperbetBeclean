using System.Windows;
using System.Windows.Controls;


namespace SuperbetBeclean.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    /// 
    public partial class LoginPage : Page
    {
        private Frame _mainFrame;

        public LoginPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void onClickLoginButton(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MainMenu(_mainFrame));
        }
    }
}
