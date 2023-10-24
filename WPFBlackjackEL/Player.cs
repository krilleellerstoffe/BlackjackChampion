using System.ComponentModel.DataAnnotations;

namespace WPFBlackjackEL
{
    public class Player
    {
        private Hand _hand = new Hand();
        private bool _isWinner;
        private int _funds;
        private PlayerStates _playerState;
        private int _playerNumber;
        private string _playerName;
        private bool _isDealer = false;

        public Player()
        {
            _playerState = PlayerStates.Waiting;
        }

        public Hand Hand { get => _hand; set => _hand = value; }
        public bool IsWinner { get => _isWinner; set => _isWinner = value; }
        [Required]
        public int Funds { get => _funds; set => _funds = value; }
        public PlayerStates PlayerState { get => _playerState; set => _playerState = value; }
        [Key]
        public string PlayerName { get => _playerName; set => _playerName = value; }
        public int PlayerNumber { get => _playerNumber; set => _playerNumber = value; }
        public bool IsDealer { get => _isDealer; set => _isDealer = value; }

        public override string ToString()
        {
            return PlayerName + " (" + Funds + " gold)";
        }


        public enum PlayerStates
        {
            Waiting,
            Standing,
            Bust,
            InPlay,
            OutOfPlay,
            Winner
        }
    }
}
