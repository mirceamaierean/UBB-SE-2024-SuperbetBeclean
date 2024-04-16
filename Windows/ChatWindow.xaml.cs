using SuperbetBeclean.Services;
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
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        ChatService _chatService;
       
        public ChatMessages Messages { get; set; } = new ChatMessages();


        public ChatWindow(ChatService chatService)
        {
            InitializeComponent();
            _chatService = chatService;
            
            DataContext = Messages;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void  Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {   
                if (e.GetPosition(this).Y < 60) // Assuming the height of the upper part is 60 (adjust as needed)
                {
                    DragMove();
                }
            }
            catch { };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //messagingBox.AppendText(chatInputTextBox.Text + "\n");
            _chatService.newMessage(chatInputTextBox.Text + "\n");
        }

        private void messagingBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
