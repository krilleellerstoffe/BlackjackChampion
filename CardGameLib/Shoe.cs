using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLib
{
    public class Shoe
    {
        private List<Card> _cards = new List<Card>();
        private int _totalCards = 0;
        private int _usedCards = 0;

        public int TotalCards { get => _totalCards; set => _totalCards = value; }
        public int UsedCards { get => _usedCards; set => _usedCards = value; }

        public Shoe(Deck[] decks)
        {
            AddDecksToShoe(decks);
            Shuffle();
        } 
        public void AddDecksToShoe(Deck[] decks)
        {
            foreach (Deck deck in decks)
            {
                foreach (Card card in deck.Cards)
                {
                    _cards.Add(card);
                }
            }
            TotalCards = _cards.Count;
        }
        //checks if it's time to shuffle based on proportion of non-shuffled cards(4 = 1/4 = 25%; 10 = 1/10 = 10%...)
        public bool TimeToShuffle(int denominator)
        {
            return UsedCards > (TotalCards - TotalCards / denominator);
        }
        //only call this method when all cards returned to deck, resets used-card-counter
        public bool Shuffle()
        {   
            if(_cards.Count != TotalCards) return false;
            _cards.OrderBy(randomValue => new Guid());
            UsedCards = 0;
            return true;
        }
        //if cards exist in the list, removes and return the last one
        public Card drawCard()
        {
            try
            {
                Card nextCard = _cards[_cards.Count -1];
                _cards.RemoveAt(_cards.Count -1);
                UsedCards++;
                return nextCard;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            
            }
        }
    }
}
