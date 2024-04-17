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

        public MainViewModel()
        {
            Balance = 12345; // TODO: Placeholder balance, be sure to change it once the backend is implemented
            ShopItems = new List<ShopItem>();
            _dbService = new DBService();
            LoadItems();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadItems()
        {
            // TODO: Placeholder items, be sure to change them once the backend is implemented
            ShopItems = _dbService.GetShopItems(); 
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