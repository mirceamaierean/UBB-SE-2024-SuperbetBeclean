using SuperbetBeclean.Services;
using SuperbetBeclean.Windows;
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
        public GameTablePage(Frame mainFrame, MenuWindow mainWindow, Service service)
        {
            InitializeComponent();
            Loaded += GameTablePage_Loaded;
            _mainWindow = mainWindow;
            _service = service;
            PlayerNameTextBox.Text = _mainWindow.userName();
            PlayerLvlTextBox.Text = _mainWindow.userLevel().ToString();
            PlayerChipsTextBox.Text = _mainWindow.userChips().ToString();
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
    }
}
