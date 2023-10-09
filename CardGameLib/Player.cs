using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLib
{
    public class Player
    {
        private Hand _hand = new Hand();
        private bool _isWinner;
        private int _funds;

        public Hand Hand { get => _hand; set => _hand = value; }
        public bool IsWinner { get => _isWinner; set => _isWinner = value; }
        public int Funds { get => _funds; set => _funds = value; }

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
    }
}
