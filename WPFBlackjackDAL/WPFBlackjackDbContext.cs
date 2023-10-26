using Microsoft.EntityFrameworkCore;
using WPFBlackjackEL;

namespace WPFBlackjackDAL
{
    public class WPFBlackjackDbContext : DbContext
    {
        //Player profile with name/funds that can be used to start a new game
        public DbSet<PlayerProfile> PlayerProfiles { get; set; }
        //Relation table that contains everythign needed to recreate a game (Shoe, Players and their Hands with Cards)
        public DbSet<GameState> GameStates { get; set; }
        //Shoe entity contains cards
        public DbSet<Shoe> Shoes { get; set; }
        //Player entity contains a Hand
        public DbSet<Player> Players { get; set; }
        //Hand entity contains cards
        public DbSet<Hand> Hands { get; set; }
        //Cards have a suit and value
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Database=WPFBlackjack1");
        }

    }
}
