using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperbetBeclean.Services;

namespace SuperbetBeclean.Models
{
    internal class ProfileViewModel
    {
        DBService _dbService;

        public List<ShopItem> OwnedItems { get; set; }

        public ProfileViewModel()
        {
            OwnedItems = new List<ShopItem>();
            _dbService = new DBService();
            LoadItems();
        }

        private void LoadItems()
        {
            // TODO: Switch to owned items once that is implemented
            OwnedItems = _dbService.GetShopItems();
        }
    }
}
