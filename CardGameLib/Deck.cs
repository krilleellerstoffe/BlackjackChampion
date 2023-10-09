using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLib
{
    public class Deck
    {
        private List<Card> _cards;

        internal List<Card> Cards { get => _cards; set => _cards = value; }

        public Deck ()
        {            
            _cards = new List<Card> ();
            createDeck();
        }

        private void createDeck()
        {
            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach (Values value in Enum.GetValues(typeof(Values)))
                {
                    _cards.Add(new Card(suit, value));
                }
            }
        }
    }
}
