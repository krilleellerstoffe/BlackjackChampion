using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLib
{
    public class Pot
    {
        private int _potTotal = 0;
        private int _currentBet = 0;

        public int CurrentBet { get => _currentBet; set => _currentBet = value; }

        public int AddToPot(int betAmount)
        {
            _potTotal += betAmount;
            return _potTotal;
        }

        public void ResetPot()
        {
            _potTotal = 0;
            _currentBet = 0;
        }
    }
}
