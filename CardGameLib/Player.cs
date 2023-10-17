namespace CardGameLib
{
    public class Player
    {
        private Hand _hand = new Hand();
        private bool _isWinner;
        private int _funds;
        private PlayerStates _playerState;
        private int _playerNumber;
        private string _playerName;
        private GameManager gameManager;

        public Player(GameManager gameManager)
        {
            this.gameManager = gameManager;
            _playerState = PlayerStates.Waiting;
            _funds = 100;
        }

        public Hand Hand { get => _hand; set => _hand = value; }
        public bool IsWinner { get => _isWinner; set => _isWinner = value; }
        public int Funds { get => _funds; set => _funds = value; }
        public PlayerStates PlayerState { get => _playerState; set => _playerState = value; }
        public string PlayerName { get => _playerName; set => _playerName = value; }
        public int PlayerNumber { get => _playerNumber; set => _playerNumber = value; }

        public void Stand()
        {

        }

        public void Double()
        {

        }
        public void Hit()
        {

        }
        public void Surrender()
        {

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
