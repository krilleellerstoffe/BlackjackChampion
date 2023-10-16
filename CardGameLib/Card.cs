namespace CardGameLib
{
    public class Card
    {
        private Suits _suit;
        private Values _value;
        private string _imagePath;

        public Card(Suits suit, Values value)
        {
            _suit = suit;
            _value = value;
            //not sure whether this should be in the card or if image logic should be put in GUI
            _imagePath = value.ToString() + suit.ToString() + ".png";
        }

        public string ImageFile { get => _imagePath; set => _imagePath = value; }
        internal Suits Suit { get => _suit; set => _suit = value; }
        internal Values Value { get => _value; set => this._value = value; }

        public override string ToString()
        {
            return Value + " of " + Suit.ToString();
        }
    }
}
