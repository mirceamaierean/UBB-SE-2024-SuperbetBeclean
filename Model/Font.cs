using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Model
{
    public class Font
    {
        private int _fontID;
        private string _fontName;
        private int _fontPrice;
        private string _fontType;

        public Font(int fontID = 0,  string fontName = "", int fontPrice = 0, string fontType = "")
        {
            _fontID = fontID;
            _fontName = fontName;
            _fontPrice = fontPrice;
            _fontType = fontType;
        }

        public int FontID { get { return _fontID; } set { _fontID = value; } }
        public string FontName { get { return _fontName;} set { _fontName = value; } }
        public int FontPrice { get { return _fontPrice;} set { _fontPrice = value; } }
        public string FontType { get { return _fontType; } set { _fontType = value; } }

    }
}
