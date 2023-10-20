using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBlackjackEL;

namespace WPFBlackjackDAL
{
    public class WPFBlackjackDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().ToTable("Players");
            modelBuilder.Entity<Card>().ToTable("Cards");

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.PlayerId)
                .IsUnique();
        }
    }
}
