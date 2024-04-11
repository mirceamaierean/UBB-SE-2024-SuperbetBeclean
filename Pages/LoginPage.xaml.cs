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
        private MainWindow _mainWindow;

        public LoginPage(Frame mainFrame, MainWindow mainWindow)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _mainWindow = mainWindow;
        }

        private void onClickLoginButton(object sender, RoutedEventArgs e)
        {

            _mainWindow.openNewWindow(inputNameLoginFirstPage.Text);
            inputNameLoginFirstPage.Text = "";
        }
    }
}
