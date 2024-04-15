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
using System.Windows.Shapes;

namespace SuperbetBeclean.Windows
{
    /// <summary>
    /// Interaction logic for ChallengesWindow.xaml
    /// </summary>
    public partial class ChallengesWindow : Window
    {
        public ChallengesWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {


                if (e.GetPosition(this).Y < 60) // Assuming the height of the upper part is 60 (adjust as needed)
                {
                    // Drag the window
                    DragMove();
                }
            }
            catch { };  
    }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the window when the button is clicked
        }
    }
    
}
