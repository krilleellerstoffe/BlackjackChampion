using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace WPFBlackjackEL
{
    public class Shoe
    {
        private List<Card> _cards = new List<Card>();
        private int _totalCards = 0;
        private int _cardsSinceLastShuffle = 0;
        private int _shoeID;

        public List<Card> Cards { get => _cards; set => _cards = value; }
        public int CardsSinceLastShuffle { get => _cardsSinceLastShuffle; set => _cardsSinceLastShuffle = value; }
        public int TotalCards { get => _totalCards; set => _totalCards = value; }
        [Key]
        public int ShoeID { get => _shoeID; set => _shoeID = value; }

        public Shoe() { }
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
            TotalCards = Cards.Count;

        }
        //checks if it's time to shuffle based on proportion of non-shuffled cards(4 = 1/4 = 25%; 10 = 1/10 = 10%...)
        public bool TimeToShuffle(int denominator)
        {
            return CardsSinceLastShuffle > TotalCards - TotalCards / denominator;
        }
        //only call this method when all cards returned to deck, resets used-card-counter
        public bool Shuffle()
        {
            //_cards.OrderBy(randomValue => new Guid());
            var rnd = new Random();
            foreach (Card card in _cards)
            {
                Debug.WriteLine(card);
            }
            var shuffledCards = Cards.OrderBy(item => rnd.Next());
            _cards = shuffledCards.ToList();
            foreach (Card card in _cards)
            {
                Debug.WriteLine(card);
            }
            CardsSinceLastShuffle = 0;
            return true;
        }
        public bool ReturnToShoe(Card[] cards)
        {
            foreach (Card card in cards)
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
