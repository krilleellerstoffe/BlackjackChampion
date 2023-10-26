using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPFBlackjackEL
{
    public class Card
    {
        private Suits _suit;
        private Values _value;
        private string _imagePath;
        private int _cardID;
        public Card(Suits suit, Values value)
        {
            _suit = suit;
            _value = value;
            //not sure whether this should be in the card or if image logic should be put in GUI
            _imagePath = value.ToString() + suit.ToString() + ".png";
        }

        public string ImageFile { get => _imagePath; set => _imagePath = value; }
        public Suits Suit { get => _suit; set => _suit = value; }
        public Values Value { get => _value; set => this._value = value; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardID { get => _cardID; set => _cardID = value; }

        public int Score()
        {
            switch (_value)
            {
                case Values.two:
                    return 2;
                case Values.three:
                    return 3;
                case Values.four:
                    return 4;
                case Values.five:
                    return 5;
                case Values.six:
                    return 6;
                case Values.seven:
                    return 7;
                case Values.eight:
                    return 8;
                case Values.nine:
                    return 9;
                case Values.ten:
                case Values.jack:
                case Values.queen:
                case Values.king:
                    return 10;
                case Values.ace:
                    return 11;
                default: return 0;
            }

        }

        public override string ToString()
        {
            return Value + " of " + Suit.ToString();
        }
    }
}
