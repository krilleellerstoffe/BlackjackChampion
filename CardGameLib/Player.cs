namespace CardGameLib
{
    public class Player
    {
        private Hand _hand = new Hand();
        private bool _isWinner;
        private int _funds;
        private PlayerStates _playerState;
        private GameManager gameManager;

        public Player(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public Hand Hand { get => _hand; set => _hand = value; }
        public bool IsWinner { get => _isWinner; set => _isWinner = value; }
        public int Funds { get => _funds; set => _funds = value; }
        public PlayerStates PlayerState { get => _playerState; set => _playerState = value; }

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
            Standing,
            Bust,
            InPlay
        }
    }
}
