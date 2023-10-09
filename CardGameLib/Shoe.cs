using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLib
{
    public class Shoe
    {
        private List<Card> _cards = new List<Card>();
        private int _totalCards = 0;
        private int _cardsSinceLastShuffle = 0;

        public List<Card> Cards { get => _cards; set => _cards = value; }
        public int CardsSinceLastShuffle { get => _cardsSinceLastShuffle; set => _cardsSinceLastShuffle = value; }

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
                    Cards.Add(card);
                }
            }
            _totalCards = Cards.Count;
            
        }
        //checks if it's time to shuffle based on proportion of non-shuffled cards(4 = 1/4 = 25%; 10 = 1/10 = 10%...)
        public bool TimeToShuffle(int denominator)
        {
            return CardsSinceLastShuffle > _totalCards - _totalCards / denominator;
        }
        //only call this method when all cards returned to deck, resets used-card-counter
        public bool Shuffle()
        {   
            //_cards.OrderBy(randomValue => new Guid());
            var rnd = new Random();
            var shuffledCards = Cards.OrderBy(item => rnd.Next());
            _cards = shuffledCards.ToList();
            CardsSinceLastShuffle = 0;
            return true;
        }
        public bool ReturnToShoe(Card[] cards)
        {
            foreach(Card card in cards)
            {
                try
                {
                    Cards.Add(card);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                }
            }
            return true;
        }
        //if cards exist in the list, removes and returns the first one
        public Card drawCard()
        {
            try
            {
                Card nextCard = Cards[0];
                Cards.RemoveAt(0);
                CardsSinceLastShuffle++;
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
