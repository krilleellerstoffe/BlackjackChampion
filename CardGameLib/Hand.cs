using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLib
{
    public class Hand
    {
        private List<Card> _cards = new List<Card> ();

        public List<Card> Cards { get => _cards; set => _cards = value; }

        public void AddToHand(Card card)
        {
            _cards.Add(card);
        }
        public bool ClearHand()
        {
            _cards.Clear();
            return true;
        }

        public int HandValue()
        {
            int handValue = 0;
            foreach(Card card in _cards)
            {
                handValue += ((int)card.Value);
            }
            return handValue;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Card card in _cards)
            {
                sb.Append(card.ToString());
            }
            return sb.ToString();
        }

    }
}
