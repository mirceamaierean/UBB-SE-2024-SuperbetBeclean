﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperbetBeclean.Model;
using SuperbetBeclean.Services;
using SuperbetBeclean.Windows;

namespace SuperbetBeclean.Models
{
    internal class ProfileViewModel
    {
        DBService _dbService;

        MenuWindow _mainWindow;
        public List<ShopItem> OwnedItems { get; set; }

        public ProfileViewModel(MenuWindow mainWindow)
        {
            _mainWindow = mainWindow;
            OwnedItems = new List<ShopItem>();
            _dbService = new DBService();
            LoadItems();
        }

        private void LoadItems()
        {
            List<ShopItem> _ownedItems = _dbService.GetAllUserIconsByUserId(_mainWindow.userId());
            foreach (var item in _ownedItems)
            {
                item.UserId = _mainWindow.userId();
                OwnedItems.Add(item);
            }
        }
    }
}
