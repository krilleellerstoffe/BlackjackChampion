using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml.Linq;
using System;
using System.IO;
using System.Windows;
using WPFBlackjackDAL;
using WPFBlackjackEL;
using static System.Net.Mime.MediaTypeNames;

namespace CardGameLib
{
    public class GameManager
    {
        private StateofPlay _state;
        private Deck[] _decks;
        private Player[] _players;
        private Shoe _shoe;
        private int _pot;
        private int _defaultFunds = 100;
        private int _betAmount = 10; //hardcoded for now, will add option to change;
        private bool _winnersDeclared = false;

        public delegate void CardDrawnHandler(Player player, Card card);
        public event CardDrawnHandler CardDrawn;
        public delegate void StandHandler(Player player);
        public event StandHandler Standing;
        public delegate void BustHandler(Player player);
        public event BustHandler Bust;
        public delegate void ResultsHandler(List<Player> winners);
        public event ResultsHandler Results;

        //create new game
        public GameManager(int deckCount, int playerCount, string[] names)
        {
            SetDecks(deckCount);
            SetPlayers(playerCount, names);
            SetupLoggers();
            _shoe = new Shoe(_decks);
            _pot = 0;
        }
        //create game from saved gamestate
        public GameManager(GameState gameState)
        {
            _players = gameState.Players.ToArray();
            _pot = gameState.Pot;
            _shoe = gameState.Shoe;
            _state = gameState.State;
            SetupLoggers();
        }
        //update player details
        public void insertPlayer(int playerNumber, Player player)
        {
            try
            {
                _players[playerNumber] = player;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
        }
        //create new decks
        private void SetDecks(int deckCount)
        {
            _decks = new Deck[deckCount];
            for (int i = 0; i < deckCount; i++)
            {
                _decks[i] = new Deck();
            }
        }
        //create players, their names, and give them some funds
        private void SetPlayers(int playerCount, string[] aiNames)
        {
            _players = new Player[playerCount + 1];
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = new Player();
                _players[i].PlayerNumber = i;
                //player[0] is always the dealer
                if (i == 0)
                {
                    _players[i].PlayerName = "Dealer";
                    _players[i].IsDealer = true;
                    continue;
                }
                _players[i].PlayerName = "AI-" + aiNames[i];
                _players[i].Funds = _defaultFunds;

            }
        }
        //set up event and error loggers
        private void SetupLoggers()
        {
            //first create loggers that subscribe to static Logger actions
            ConsoleLogger consoleLogger = new ConsoleLogger();
            FileLogger fileLogger = new FileLogger("log.txt");
            //now subscribe trigger static Logger actions by subscribing to game events
            CardDrawn += (player, card) => Logger.LogMessage(player.PlayerName + " received a " + card.ToString());
            Standing += (player) => Logger.LogMessage(player.PlayerName + " is standing with " + player.Hand.HandValue());
            Bust += (player) => Logger.LogMessage(player.PlayerName + " went bust with " + player.Hand.HandValue());
            Results += LogWinners;
        }
        //create a string of winners to send to the logger
        private void LogWinners(List<Player> winners)
        {
            string winnerString = "";
            foreach (Player player in winners)
            {
                winnerString += player.PlayerName + "\n";
            }
            Logger.LogMessage("Winners:\n" + winnerString);
        }
        //hit each player twice on dealing
        public void Deal()
        {
            foreach (Player player in _players)
            {
                if (player.PlayerState == Player.PlayerStates.InPlay) Hit(player);
            }
            foreach (Player player in _players)
            {
                if (player.PlayerState == Player.PlayerStates.InPlay) Hit(player);
            }
        }
        public void Stand(int playerNumber)
        {
            //only change state to standing if not bust, declared winner, or surrendered
            if (_players[playerNumber].PlayerState == Player.PlayerStates.InPlay)
            {
                _players[playerNumber].PlayerState = Player.PlayerStates.Standing;
                Standing(_players[playerNumber]);
            }
            //if it's the dealer calling stand, set winners and exit loop
            if (playerNumber == 0)
            {
                if (_winnersDeclared) return;
                SetWinners();
                return;
            }
            //next player's turn to hit (if they exist)
            if (_players.Length > playerNumber + 1) AIHit(playerNumber + 1);
            //otherwise let the dealer finish
            AIHit(0);
        }
        //sets winners based on hand value or blackjack found
        private void SetWinners()
        {
            int winningScore = 0;
            List<Player> winners = new List<Player>();
            //first find the highest hand score
            for (int i = 0; i < _players.Length; i++)
            {
                int handValue = _players[i].Hand.HandValue();
                //check player is not bust or out of play
                if ((handValue > winningScore) && _players[i].PlayerState != Player.PlayerStates.Bust && _players[i].PlayerState != Player.PlayerStates.OutOfPlay)
                {
                    winningScore = handValue;
                }
            }
            //now set all who have high score to winners
            for (int i = 0; i < _players.Length; i++)
            {
                int handValue = _players[i].Hand.HandValue();
                //check player is not bust or out of play
                if ((handValue == winningScore) && _players[i].PlayerState != Player.PlayerStates.Bust && _players[i].PlayerState != Player.PlayerStates.OutOfPlay)
                {
                    _players[i].PlayerState = Player.PlayerStates.Winner;
                    winners.Add(_players[i]);
                }
            }
            _winnersDeclared = true;
            if (Results != null) Results(winners);
        }
        //give player a new card from the shoe
        public void Hit(Player player)
        {
            Card newCard = _shoe.drawCard();
            player.Hand.AddToHand(newCard);
            if (CardDrawn != null) CardDrawn(player, newCard);
            CheckIfBust(player);
        }
        //AI will hit if hand under 18, otherwise stand
        public void AIHit(int playerNumber)
        {
            Player AIPlayer = _players[playerNumber];
            while (AIPlayer.Hand.HandValue() < 18) Hit(AIPlayer);
            Stand(playerNumber);
        }
        //set player as bust and then stand to allow next player to act
        private void CheckIfBust(Player player)
        {
            if (player.Hand.HandValue() > 21)
            {
                player.PlayerState = Player.PlayerStates.Bust;
                if (Bust != null) Bust(player);
                Stand(1);
            }

        }
        //split winnings, clear hands and take new bets
        public void NewHand()
        {
            _winnersDeclared = false;
            foreach (Player player in _players)
            {
                _shoe.ReturnToShoe(player.Hand.Cards.ToArray());
                player.Hand.ClearHand();
                if (player.IsDealer)
                {
                    _pot += _betAmount;
                    player.PlayerState = Player.PlayerStates.InPlay;
                }
                else if (player.Funds >= _betAmount)
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
            State = StateofPlay.NewHand;
        }
        //pot gets split evenly between winners
        public void SplitPotToWinners()
        {
            int winners = 0;
            //first count how many winners there are
            foreach (Player player in _players)
            {
                if (player.PlayerState == Player.PlayerStates.Winner) winners++;
            }
            //now split pot
            foreach (Player player in _players)
            {
                if (player.IsDealer) continue;
                if (player.PlayerState == Player.PlayerStates.Winner) player.Funds += _pot / winners;
            }
            _pot = 0;
        }
        //get half bet back if giving up straight after deal
        public void Surrender()
        {
            _players[1].PlayerState = Player.PlayerStates.OutOfPlay;
            _pot -= _betAmount / 2;
            _players[1].Funds += _betAmount / 2;
            Stand(1);
        }
        //Saves the current user's profile to the database
        public void SavePlayerToDatabase()
        {
            using WPFBlackjackDbContext context = new WPFBlackjackDbContext();
            try
            {
                // Retrieve the player you want to update
                PlayerProfile currentProfile = new PlayerProfile(_players[1].PlayerName, _players[1].Funds);
                PlayerProfile? profileToUpdate = context.PlayerProfiles.FirstOrDefault(p => p.PlayerName == currentProfile.PlayerName);

                if (profileToUpdate != null)
                {
                    // Modify the player's funds
                    profileToUpdate.Funds = currentProfile.Funds;
                }
                else
                {
                    //otherwise create a new entry in the database
                    context.Add(currentProfile);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        //retrieve a list of profiles that are saved in the database
        public static List<PlayerProfile> GetPlayersFromDatabase()
        {
            using WPFBlackjackDbContext context = new WPFBlackjackDbContext();
            List<PlayerProfile> players = (from player in context.PlayerProfiles
                                           select player).ToList();
            return players;
        }
        //delete a player progile from the database
        public static void RemovePlayerFromDatabase(Player selectedPlayer)
        {
            using WPFBlackjackDbContext context = new WPFBlackjackDbContext();
            context.Remove(selectedPlayer);
            context.SaveChanges();
        }
        //create a gamestate and save it to the database
        public void SaveGame()
        {
            using WPFBlackjackDbContext context = new WPFBlackjackDbContext();
            GameState gameState = new GameState(_shoe, _players, _pot, _state);
            //make sure each item is saved correctly by explicitly adding items
            foreach (Card card in Shoe.Cards)
            {
                context.Add(card);
            }
            context.Add(_shoe);
            foreach (Player player in _players)
            {
                foreach (Card card in player.Hand.Cards)
                {
                    context.Add(card);
                }
                context.Add(player.Hand);
                context.Add(player);
            }
            context.Add(gameState);
            context.SaveChanges();
        }
        //create a new gamemanager based on the loaded gamestate
        public static GameManager LoadGame(GameState savedGame)
        {
            return new GameManager(savedGame);
        }
        //retrieve a list of gamestates saved in the database
        public static List<GameState> GetSaveGamesFromDatabase()
        {
            using WPFBlackjackDbContext context = new WPFBlackjackDbContext();
            //explicity load entities to defeat lazy loading
            List<GameState> saveGames = context.GameStates
                .Include(gs => gs.Shoe)
                    .ThenInclude(shoe => shoe.Cards)
                .Include(gs => gs.Players)
                    .ThenInclude(player => player.Hand)
                        .ThenInclude(hand => hand.Cards)
                .ToList();
            return saveGames;
        }
        //remove a saved gamestate from the database
        public static void RemoveSaveFromDatabase(GameState saveGame)
        {
            using WPFBlackjackDbContext context = new WPFBlackjackDbContext();
            context.Remove(saveGame);
            context.SaveChanges();
        }
        public Player[] Players { get => _players; set => _players = value; }
        public Shoe Shoe { get => _shoe; set => _shoe = value; }
        public int Pot { get => _pot; set => _pot = value; }
        public StateofPlay State { get => _state; set => _state = value; }
    }
}
