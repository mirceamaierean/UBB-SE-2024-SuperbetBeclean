using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperbetBeclean.Model
{
    public class Deck
    {
        const string HEART = "H", DIAMOND = "D", SPADE = "S", CLUB = "C";

        private List<Card> deck;

        public Deck()
        {
            deck = new List<Card>();

            for (int cardValue = 2; cardValue <= 10; cardValue++)
                addCardsForValue(cardValue.ToString());

            addCardsForValue("J");
            addCardsForValue("Q");
            addCardsForValue("K");
            addCardsForValue("A");
        }
        public void addCardsForValue(string value)
        {
            deck.Add(new Card(value, HEART));
            deck.Add(new Card(value, DIAMOND));
            deck.Add(new Card(value, SPADE));
            deck.Add(new Card(value, CLUB));
        }

        public void removeCardFromIndex(int index)
        {
            deck.RemoveAt(index);
        }
        public Card getCardFromIndex(int index)
        {
            return deck[index];
        }

        public int getDeckSize()
        {
            return deck.Count;
        }

    }
}
