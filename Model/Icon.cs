using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Model
{
    public class Icon
    {
        private int _iconID;
        private string _iconName;
        private int _iconPrice;
        private string _iconPath;

        public Icon(int iconID = 0, string iconName = "", int iconPrice = 0, string iconPath = "")
        {
            _iconID = iconID;
            _iconName = iconName;
            _iconPrice = iconPrice;
            _iconPath = iconPath;
        }

        public int IconID { get { return _iconID; } set { _iconID = value;  } }
        public string IconName { get { return _iconName; } set { _iconName = value; } }
        public int IconPrice { get { return _iconPrice; } set { _iconPrice = value; } }
        public string IconPath { get { return _iconPath; } set { _iconPath = value; } }

    }
}
