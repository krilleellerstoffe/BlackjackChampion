using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WPFBlackjackEL
{
    public class Hand
    {
        private List<Card> _cards = new List<Card>();
        private int _handValue = 0;
        private bool _isSoft = false;   //soft if using an ace worth 1 point
        private bool _isBlackJack = false;
        private int _handId;

        public List<Card> Cards { get => _cards; set => _cards = value; }
        public bool IsBlackJack { get => _isBlackJack; set => _isBlackJack = value; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HandId { get => _handId; set => _handId = value; }

        public void AddToHand(Card card)
        {
            _cards.Add(card);

        }
        public bool ClearHand()
        {
            _cards.Clear();
            _handValue = 0;
            _isSoft = false;
            return true;
        }

        public int HandValue()
        {
            _handValue = 0;
            if (_cards.Count == 2)
            {
                if (CheckIfBlackJack())
                {
                    _isBlackJack = true;
                    return 21;
                }
            }
            _isBlackJack = false;
            foreach (Card card in _cards)
            {
                _handValue += card.Score();
            }
            SoftenHand();

            return _handValue;
        }

        private bool CheckIfBlackJack()
        {
            bool jack = false;
            bool ace = false;
            foreach (Card card in _cards)
            {
                if (card.Value == Values.jack)
                {
                    jack = true;
                }
                else if (card.Value == Values.ace)
                {
                    ace = true;
                }
            }
            return jack && ace;

        }

        private void SoftenHand()
        {
            if (_handValue <= 21) return;
            int aces = 0;
            _isSoft = true;
            foreach (Card card in _cards)
            {
                if (card.Score() == 11)
                {
                    aces++;
                }
            }
            for (int i = 0; i < aces; i++)
            {
                _handValue -= 10;
                if (_handValue <= 21) return;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Card card in _cards)
            {
                sb.Append(card.ToString());
            }
            return sb.ToString();
        }

    }
}
