using System.ComponentModel.DataAnnotations;

namespace WPFBlackjackEL
{
    public class PlayerProfile
    {
        private string playerName;
        private int funds;

        public PlayerProfile(string playerName, int funds)
        {
            this.PlayerName = playerName;
            this.Funds = funds;
        }
        [Key]
        public string PlayerName { get => playerName; set => playerName = value; }
        public int Funds { get => funds; set => funds = value; }
    }
}