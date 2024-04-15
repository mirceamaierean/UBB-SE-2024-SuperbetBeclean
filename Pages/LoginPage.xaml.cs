using System.Diagnostics;
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
        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            // Clear the text when the TextBox gets focus
            TextBox textBox = sender as TextBox;
            textBox.Text = "";
            
        }

        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            // Restore the placeholder text if the TextBox loses focus and is empty
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Input your name";
            }
        }

        private void onClickLoginButton(object sender, RoutedEventArgs e)
        {
            _mainWindow.openNewWindow(inputNameLoginFirstPage.Text);
            inputNameLoginFirstPage.Text = "";
        }
    }
}
