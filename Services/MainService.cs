using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SuperbetBeclean.Windows;

namespace SuperbetBeclean.Services
{
    public class MainService
    {
        private List < Window > openedUsersWindows;

        public MainService()
        {
            openedUsersWindows = new List <Window>();
        }

        public void addWindow(string username)
        {
            MenuWindow menuWindow = new MenuWindow(username);
            menuWindow.Show();
            openedUsersWindows.Add(menuWindow);
        }
    }
}
