using SuperbetBeclean.Model;
using SuperbetBeclean.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperbetBeclean.Models
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _balance;

        DBService _dbService;

        public int Balance
        {
            get { return _balance; }
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        public List<ShopItem> ShopItems { get; set; }

        public MainViewModel(int currentBalance, int userId)
        {
            Balance = currentBalance;
            ShopItems = new List<ShopItem>();
            _dbService = new DBService();
            LoadItems(userId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadItems(int userId)
        {
            List<ShopItem> ownedItems = _dbService.GetAllUserIconsByUserId(userId);
            List<ShopItem> allItems = _dbService.GetShopItems();

            // Use a HashSet to store the names of owned items for fast lookup
            HashSet<string> ownedItemNames = new HashSet<string>();
            foreach (var item in ownedItems)
            {
                ownedItemNames.Add(item.Name);
            }

            // Initialize the list for purchasable items
            List<ShopItem> purchasableItems = new List<ShopItem>();

            // Check each item in the shop to see if it's not owned by the user
            foreach (var item in allItems)
            {
                if (!ownedItemNames.Contains(item.Name))
                {
                    purchasableItems.Add(item);
                }
            }

            ShopItems = purchasableItems;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

// TODO: Delete once the commands are implemented
public class PlaceholderCommand : ICommand
{
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {

    }
}