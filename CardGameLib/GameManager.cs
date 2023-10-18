using System.Diagnostics;
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
        private bool _winnersDeclared = false;

        public delegate void CardDrawnHandler(Player player, Card card);
        public event CardDrawnHandler CardDrawn;
        public delegate void StandHandler (Player player);
        public event StandHandler Standing;
        public delegate void BustHandler(Player player);
        public event BustHandler Bust;
        public delegate void ResultsHandler(List<Player> winners);
        public event ResultsHandler Results;

        private ConsoleLogger consoleLogger;
        private FileLogger fileLogger;

        public GameManager(int deckCount, int playerCount)
        {
            SetupLoggers();
            SetDecks(deckCount);
            SetPlayers(playerCount);

            _shoe = new Shoe(this, _decks);
            _pot = 0;
            _betAmount = 10; //hardcoded for now, will add option to change
        }

        private void SetupLoggers()
        {
            consoleLogger = new ConsoleLogger();
            fileLogger = new FileLogger("log.txt");
            CardDrawn += (player, card) => Logger.LogMessage(player.PlayerName + " received a " + card.ToString());
            Standing += (player) => Logger.LogMessage(player.PlayerName + " is standing with " + player.Hand.HandValue());
            Bust += (player) => Logger.LogMessage(player.PlayerName + " went bust with " + player.Hand.HandValue());
            Results += LogWinners;
        }

        private void LogWinners(List<Player> winners)
        {
            string winnerString = "";
            foreach (Player player in winners)
            {
                winnerString += player.PlayerName + "\n";
            }
            Logger.LogMessage("Winners:\n" + winnerString);
        }

        private void SetDecks(int deckCount)
        {
            _decks = new Deck[deckCount];
            for (int i = 0; i < deckCount; i++)
            {
                _decks[i] = new Deck();
            }
        }

        private void SetPlayers(int playerCount)
        {
            //player[0] is always the dealer
            _players = new Player[playerCount + 1];
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = new Player(this);
                _players[i].PlayerNumber = i;
                _players[i].PlayerName = "Player " + i;
                if (i == 0) _players[i].PlayerName = "Dealer";
            }
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
            //only change state to standing if not bust, declared winner, or surrendered
            if (_players[playerNumber].PlayerState == Player.PlayerStates.InPlay)
            {
                _players[playerNumber].PlayerState = Player.PlayerStates.Standing;
            }
            //if it's the dealer calling stand, set winners and exit loop
            if (playerNumber == 0)
            {
                if (_winnersDeclared) return;
                SetWinners();
                return;
            }
            //next player hit/stand (if exists)
            if (_players.Length > playerNumber + 1)
            {
                AIHit(playerNumber + 1);
            }
            //otherwise let the dealer finish
            AIHit(0);
        }

        private void SetWinners()
        {
            int winningScore = 0;
            List<Player> winners = new List<Player>();
            for (int i = 0; i < _players.Length; i++)
            {
                int handValue = _players[i].Hand.HandValue();
                if ((handValue > winningScore) && _players[i].PlayerState != Player.PlayerStates.Bust && _players[i].PlayerState != Player.PlayerStates.OutOfPlay)
                {
                    winningScore = handValue;
                }
            }
            for (int i = 0; i < _players.Length; i++)
            {
                int handValue = _players[i].Hand.HandValue();
                if ((handValue == winningScore) && _players[i].PlayerState != Player.PlayerStates.Bust && _players[i].PlayerState != Player.PlayerStates.OutOfPlay)
                {
                    _players[i].PlayerState = Player.PlayerStates.Winner;
                    winners.Add(_players[i]);
                }
            }
            _winnersDeclared = true;
            if (Results != null)
            {
                Results(winners);
            }
            Debug.WriteLine("Winning score: " + winningScore);
            Debug.WriteLine("Winning players:");
            foreach (Player player in winners)
            {
                Debug.WriteLine(player.PlayerName);
            }

        }

        public void Hit(Player player)
        {
            Card newCard = _shoe.drawCard();
            player.Hand.AddToHand(newCard);
            if (CardDrawn != null)
            {
                CardDrawn(player, newCard);
            }
            CheckIfBust(player);
        }

        //hit if hand under 18, then stand/bust
        public void AIHit(int playerNumber)
        {
            Player AIPlayer = _players[playerNumber];
            while (AIPlayer.Hand.HandValue() < 18)
            {
                Hit(AIPlayer);
            }
            Stand(playerNumber);

        }

        private void CheckIfBust(Player player)
        {
            if (player.Hand.HandValue() > 21)
            {
                player.PlayerState = Player.PlayerStates.Bust;
                if (Bust != null)
                {
                    Bust(player);
                }
                Stand(1);
            }

        }

        public void NewHand()
        {
            SplitPotToWinners();
            _winnersDeclared = false;
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

        public bool TimeToShuffle(int denominator)
        {
            return _shoe.TimeToShuffle(denominator);
        }

        public void Surrender()
        {
            _players[1].PlayerState = Player.PlayerStates.OutOfPlay;
            _pot -= _betAmount / 2;
            _players[1].Funds += _betAmount / 2;
            Stand(1);
        }

        public Deck[] Decks { get => _decks; set => _decks = value; }
        public Player[] Players { get => _players; set => _players = value; }
        public Shoe Shoe { get => _shoe; set => _shoe = value; }
        public int Pot { get => _pot; set => _pot = value; }
    }
}
