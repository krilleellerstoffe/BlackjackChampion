using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBlackjackEL
{
    public class GameState
    {
        private Shoe _shoe;
        private Player[] _players;
        private int _pot;
        private int _gameId;

        public GameState() { }
        public GameState(Shoe shoe, Player[] players, int pot)
        {
            _shoe = shoe;
            _players = players;
            _pot = pot;
        }

        [Required]
        public Player[] Players { get => _players; set => _players = value; }
        [Required]

        public int Pot { get => _pot; set => _pot = value; }
        [Key]
        public int GameId { get => _gameId; set => _gameId = value; }
        [Required]

        public Shoe Shoe { get => _shoe; set => _shoe = value; }
    }
}
