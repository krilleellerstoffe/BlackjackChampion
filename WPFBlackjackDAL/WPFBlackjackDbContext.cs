using Microsoft.EntityFrameworkCore;
using WPFBlackjackEL;

namespace WPFBlackjackDAL
{
    public class WPFBlackjackDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<GameState> GameStates { get; set; }

        public DbSet<Shoe> shoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Database=WPFBlackjack");
        }
    }
}
