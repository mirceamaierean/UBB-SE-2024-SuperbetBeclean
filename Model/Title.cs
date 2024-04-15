using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Model
{
    public class Title
    {
        private int _titleID;
        private string _titleName;
        private int _titlePrice;
        private string _titleContent;

        public Title(int titleID = 0, string titleName = "", int titlePrice = 0, string titleContent = "")
        {
            _titleID = titleID;
            _titleName = titleName;
            _titlePrice = titlePrice;
            _titleContent = titleContent;
        }

        public int TitleID { get { return _titleID; } set { _titleID = value; } }
        public string TitleName { get { return _titleName; } set { _titleName = value; } }
        public int TitlePrice { get { return _titlePrice; } set { _titlePrice = value; } }
        public string TitleContent { get { return _titleContent; } set { _titleContent = value; } }

    }
}
