using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLib
{
    public class GameManager
    {
        //mayber inherit a listmanger class that can be used for both game, shoe, hand and deck?
        private Deck[] _decks;
        private Player[] _players;
        private Shoe _shoe;
        private Pot _pot;

        public GameManager(int deckCount, int playerCount) 
        {
            _decks = new Deck[deckCount];
            for (int i = 0; i < deckCount; i++)
            {
                _decks[i] = new Deck();
            }
            _players = new Player[playerCount];
            for (int i = 0;i < playerCount; i++)
            {
                _players[i] = new Player();
            }
            _shoe = new Shoe(_decks);
        }

        public void Deal()
        {
            foreach (Player player in _players)
            {
                player.Hand = new Hand();
                player.Hand.AddToHand(_shoe.drawCard());
            }
            foreach (Player player in _players)
            {
                player.Hand.AddToHand(_shoe.drawCard());
                Debug.WriteLine("Player hand: " + player.Hand.ToString());
            }
        }

        public void Hit(Player player)
        {
            player.Hand.AddToHand(_shoe.drawCard());
        }

        public Deck[] Decks { get => _decks; set => _decks = value; }
        public Player[] Players { get => _players; set => _players = value; }
        public Shoe Shoe { get => _shoe; set => _shoe = value; }
        public Pot Pot { get => _pot; set => _pot = value; }
    }
}
