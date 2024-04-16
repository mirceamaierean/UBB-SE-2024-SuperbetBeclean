using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Model
{
    public class Card
    {
        private string value, suit;

        public Card(string value, string suit)
        {
            this.value = value;
            this.suit = suit;
        }

        public string Value { get { return value; } set { value = value; } }
        public string Suit { get { return suit; } set { suit = value; } }
    }
}
