using SuperbetBeclean.Windows;
using System.Collections.Generic;
using System.Windows;

public class ChatService
{
    private static Dictionary<(MenuWindow, string), ChatWindow> menuWindowChatWindowMap;

    public ChatService()
    {
        menuWindowChatWindowMap = new Dictionary<(MenuWindow, string), ChatWindow>();
    }
    public void closeChat(MenuWindow _mainWindow)
    {
        foreach (var entry in menuWindowChatWindowMap)
        {
            var key = entry.Key;
            if (key.Item1 == _mainWindow)
            {
                entry.Value.Close();
                break;
            }
        }
    }
    public void newChat(MenuWindow _mainWindow, string _tableType)
    {
        foreach (Window window in Application.Current.Windows)
        {
            if (window.GetType() == typeof(MenuWindow) && window == _mainWindow)
            {
                MenuWindow menuWindow = (MenuWindow)window;
                var key = (menuWindow, _tableType);

                // Check if a ChatWindow is already open for this MenuWindow and tableType
                if (!menuWindowChatWindowMap.ContainsKey(key))
                {
                    ChatWindow chatWindow = new ChatWindow(this);
                    chatWindow.Owner = menuWindow;
                    chatWindow.Closed += (s, args) => menuWindowChatWindowMap.Remove(key); // Remove from dictionary when closed
                    menuWindowChatWindowMap.Add(key, chatWindow); // Add to dictionary
                    chatWindow.messagingBox.Text = _mainWindow.userName();
                    chatWindow.Show();
                }
                else
                {
                    // Bring existing ChatWindow to the front
                    ChatWindow existingChatWindow = menuWindowChatWindowMap[key];
                    existingChatWindow.Activate();
                }
            }
        }
    }

    public void newMessage(string _message, ChatWindow thisWindow)
    {
        string userName = "";
        string tableType = "";
        
        // Find the userName and tableType corresponding to this window
        foreach (var entry in menuWindowChatWindowMap)
        {
            if (entry.Value == thisWindow)
            {
                userName = entry.Key.Item1.userName();
                tableType = entry.Key.Item2;
                break;
            }
        }

        // Update all messagingBoxes with the new message
        foreach (var entry in menuWindowChatWindowMap)
        {
            var key = entry.Key;
            var chatWindow = entry.Value;

            // Check if the tableType matches the desired tableType
            if (key.Item2 == tableType)
            {
                // Update the messagingBox in chatWindow with the new message
                chatWindow.messagingBox.Text += " " + userName + " (" + tableType + "): " + _message;
            }
        }
    }
}
