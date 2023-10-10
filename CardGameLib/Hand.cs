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
        private int _handValue = 0;
        private string _handType = "hard";   //soft if using an ace worth 1 point

        public List<Card> Cards { get => _cards; set => _cards = value; }

        public void AddToHand(Card card)
        {
            _cards.Add(card);
        }
        public bool ClearHand()
        {
            _cards.Clear();
            _handValue = 0;
            return true;
        }

        public int HandValue()
        {
            _handValue = 0;
            foreach (Card card in _cards)
            {
                _handValue += ((int)card.Value);
            }
            SoftenHand();
            return _handValue;
        }
        private void SoftenHand()
        {
            if (_handValue <= 21) return;
            int aces = 0;
            foreach (Card card in _cards)
            {
                if ((int)card.Value == 11)
                {
                    aces++;
                }
            }
            for(int i = 0; i < aces; i++) 
            {
                _handValue -= 10;
                if (_handValue <= 21) return;
            }

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
