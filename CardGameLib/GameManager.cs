using System.Numerics;

namespace CardGameLib
{
    public class GameManager
    {
        //mayber inherit a listmanger class that can be used for both game, shoe, hand and deck?
        private Deck[] _decks;
        private Player[] _players;
        private Shoe _shoe;
        private int _pot;
        private int _betAmount;

        public delegate void CardDrawnHandler();
        public event CardDrawnHandler CardDrawn;
        public delegate void BustHandler(Player player);
        public event BustHandler Bust;

        public GameManager(int deckCount, int playerCount)
        {
            _decks = new Deck[deckCount];
            for (int i = 0; i < deckCount; i++)
            {
                _decks[i] = new Deck();
            }
            //player[0] is always the dealer
            _players = new Player[playerCount+1];
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = new Player(this);
            }
            _shoe = new Shoe(this, _decks);
            _pot = 0;
            _betAmount = 10; //hardcoded for now, will add option to change
        }

        public void Deal()
        {
            NewHand();
            foreach (Player player in _players)
            {
                if (player.PlayerState == Player.PlayerStates.InPlay)
                {
                    Hit(player);
                }
                
            }
            foreach (Player player in _players)
            {
                if (player.PlayerState == Player.PlayerStates.InPlay)
                {
                    Hit(player);
                }
            }
        }
        public void Stand(int playerNumber)
        {
            _players[playerNumber].PlayerState = Player.PlayerStates.Standing;
            //next player hit/stand (if exists)
            if (_players.Length > playerNumber+1)
            {
                AIHit(playerNumber + 1);
            }
            //dealer hit/stand
            DealerHit();
            //winner declared
            //points from pot
        }

        private void DealerHit()
        {
            Player dealer = _players[0];
            if (dealer.Hand.HandValue() <= 17)
            {
                Hit(dealer);
            }
        }

        public void Hit(Player player)
        {
            player.Hand.AddToHand(_shoe.drawCard());
            CardDrawn();
            CheckIfBust(player);
        }
        //hit if hand under 18, then stand/bust
        public void AIHit(int playerNumber)
        {
            Player AIPlayer = _players[playerNumber];
            if (AIPlayer.Hand.HandValue() >= 17)
            {
                Stand(playerNumber);
            }
            else
            {
                Hit(AIPlayer);
            }
            //if not bust or standing, test whether to hit again
            if (AIPlayer.PlayerState == Player.PlayerStates.InPlay)
            {
                AIHit(playerNumber);
            }
            
        }

        private void CheckIfBust(Player player)
        {
            if (player.Hand.HandValue() > 21)
            {
                player.PlayerState = Player.PlayerStates.Bust;
                Bust(player);
            }
        }

        public void NewHand()
        {
            SplitPotToWinners();
            foreach (Player player in _players)
            {
                _shoe.ReturnToShoe(player.Hand.Cards.ToArray());
                player.Hand.ClearHand();                
                if (player.Funds > _betAmount)
                {
                    player.Funds -= _betAmount;
                    _pot += _betAmount;
                    player.PlayerState = Player.PlayerStates.InPlay;
                }
                else
                {
                    player.PlayerState = Player.PlayerStates.OutOfPlay;
                }

            }
        }

        private void SplitPotToWinners()
        {
            int winners = 0;
            foreach (Player player in _players)
            {                
                if (player.PlayerState == Player.PlayerStates.Winner)
                {
                    winners++;
                }
            }
            foreach (Player player in _players)
            {
                if (player.PlayerState == Player.PlayerStates.Winner)
                {
                    player.Funds += _pot / winners;
                }
            }
            _pot = 0;
        }

        public Deck[] Decks { get => _decks; set => _decks = value; }
        public Player[] Players { get => _players; set => _players = value; }
        public Shoe Shoe { get => _shoe; set => _shoe = value; }
        public int Pot { get => _pot; set => _pot = value; }
    }
}
