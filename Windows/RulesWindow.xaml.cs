using Microsoft.Win32;
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
using System.IO;

namespace SuperbetBeclean.Windows
{
    using System;
    using System.IO;
    using System.Windows;
    /// <summary>
    /// Interaction logic for RulesWindow.xaml
    /// </summary>
    public partial class RulesWindow : Window
    {
        private const string HtmlFilePath = @".\assets\index.html";

        public RulesWindow()
        {
            InitializeComponent();
            LoadHtmlContent();
        }

        private void closeButtonRulesWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LoadHtmlContent()
        {
            string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            // Combine the solution directory with the relative HTML file path
            string htmlFilePath = Path.Combine(solutionDirectory, HtmlFilePath);



            if (File.Exists(htmlFilePath))
            {
                // Read HTML content from the file
                string htmlContent = File.ReadAllText(htmlFilePath);

                // Display the HTML content in the WebBrowser control
                WebBrowser.NavigateToString(htmlContent);
            }
            else
            {
                MessageBox.Show("Rules file not found.");
            }
        }
    }
}
