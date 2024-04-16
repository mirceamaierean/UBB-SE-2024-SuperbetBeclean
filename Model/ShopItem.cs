using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SuperbetBeclean.Models
{
    public class ShopItem
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public ShopItem(int id, string imagePath, string name, int price)
        {
            Id = id;
            ImagePath = imagePath;
            Name = name;
            Price = price;
        }

        public ICommand BuyCommand { get; set; }
    }
}