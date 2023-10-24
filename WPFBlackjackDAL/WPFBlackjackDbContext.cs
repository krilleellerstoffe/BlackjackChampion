using Microsoft.EntityFrameworkCore;
using WPFBlackjackEL;

namespace WPFBlackjackDAL
{
    public class WPFBlackjackDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<GameState> GameStates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Database=WPFBlackjack");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Hand);

            modelBuilder.Entity<GameState>()
                .HasOne(gs => gs.Shoe);

            modelBuilder.Entity<GameState>()
                .HasMany(gs => gs.Players);

            modelBuilder.Entity<Shoe>()
                .HasMany(s => s.Cards);

        }
    }
}
