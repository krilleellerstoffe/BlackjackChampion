namespace CardGameLib
{
    public class GameManager
    {
        //mayber inherit a listmanger class that can be used for both game, shoe, hand and deck?
        private Deck[] _decks;
        private Player[] _players;
        private Shoe _shoe;
        private Pot _pot;

        public delegate void CardDrawnHandler();
        public event CardDrawnHandler CardDrawn;
        public delegate void StandHandler();
        public event StandHandler Stand;
        public delegate void BustHandler();
        public event BustHandler Bust;

        public GameManager(int deckCount, int playerCount)
        {
            _decks = new Deck[deckCount];
            for (int i = 0; i < deckCount; i++)
            {
                _decks[i] = new Deck();
            }
            _players = new Player[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                _players[i] = new Player(this);
            }
            _shoe = new Shoe(this, _decks);
        }

        public void Deal()
        {
            NewHand();
            foreach (Player player in _players)
            {
                player.Hand.AddToHand(_shoe.drawCard());
                CardDrawn();
            }
            foreach (Player player in _players)
            {
                player.Hand.AddToHand(_shoe.drawCard());
                CardDrawn();
            }
        }

        public void Hit(Player player)
        {
            player.Hand.AddToHand(_shoe.drawCard());
            CardDrawn();
            CheckIfBust(player);
        }

        private void CheckIfBust(Player player)
        {
            if (player.Hand.HandValue() > 21)
            {
                Bust();
            }
        }

        public void NewHand()
        {
            foreach (Player player in _players)
            {
                _shoe.ReturnToShoe(player.Hand.Cards.ToArray());
                player.Hand.ClearHand();

            }
        }

        public Deck[] Decks { get => _decks; set => _decks = value; }
        public Player[] Players { get => _players; set => _players = value; }
        public Shoe Shoe { get => _shoe; set => _shoe = value; }
        public Pot Pot { get => _pot; set => _pot = value; }
    }
}
