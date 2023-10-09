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
            Cards.Add(card);
        }
        public Card[] ClearHand()
        {
            return Cards.ToArray ();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Card card in Cards)
            {
                sb.Append(card.ToString());
            }
            return sb.ToString();
        }

    }
}
