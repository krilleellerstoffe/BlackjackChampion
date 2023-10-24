using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPFBlackjackEL
{
    public class GameState
    {
        private Shoe _shoe;
        private List<Player> _players;
        private int _pot;
        private int _gameId;

        public GameState() { }
        public GameState(Shoe shoe, Player[] players, int pot)
        {
            _shoe = shoe;
            _players = players.ToList();
            _pot = pot;
        }

        [Required]
        public List<Player> Players { get => _players; set => _players = value; }
        [Required]

        public int Pot { get => _pot; set => _pot = value; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get => _gameId; set => _gameId = value; }
        [Required]

        public Shoe Shoe { get => _shoe; set => _shoe = value; }

        public override string ToString()
        {
            return "Game " + GameId;

        }
    }
}
