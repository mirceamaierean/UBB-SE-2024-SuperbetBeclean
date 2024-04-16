using SuperbetBeclean.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SuperbetBeclean.Services
{   /// <summary>
    /// 3 vectori pt fiecare table
    /// new chat window sa populezi cu mesajele alea
    /// 3 vectori pt fiecare table cu window urile ca sa le parsezi pe alea deschise sa repopulezi cu mesaje
    /// </summary>
    public class ChatService
    {
        private Dictionary<MenuWindow, ChatWindow> menuWindowChatWindowMap;

        public ChatService()
        {
            menuWindowChatWindowMap = new Dictionary<MenuWindow, ChatWindow>();
        }
        
        public void newChat(MenuWindow _mainWindow)
        {

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MenuWindow) && window == _mainWindow)
                {
                    MenuWindow menuWindow = (MenuWindow)window;
                    // Check if a ChatWindow is already open for this MenuWindow
                    if (!menuWindowChatWindowMap.ContainsKey(menuWindow))
                    {
                        ChatWindow chatWindow = new ChatWindow(this);
                        chatWindow.Owner = menuWindow;
                        chatWindow.Closed += (s, args) => menuWindowChatWindowMap.Remove(menuWindow); // Remove from dictionary when closed
                        menuWindowChatWindowMap.Add(menuWindow, chatWindow); // Add to dictionary
                        chatWindow.messagingBox.Text = _mainWindow.userName();
                        chatWindow.Show();
                    }
                    else
                    {
                        // Bring existing ChatWindow to the front
                        ChatWindow existingChatWindow = menuWindowChatWindowMap[menuWindow];
                        existingChatWindow.Activate();
                    }
                }
            }
        }
        public void newMessage(string _message)
        {
            foreach (var chatWindow in menuWindowChatWindowMap.Values)
            {
                //chatWindow.messagingBox.AppendText(_message + "\n");
                chatWindow.Messages.Message += _message + "\n";

            }
        }
    }
}
