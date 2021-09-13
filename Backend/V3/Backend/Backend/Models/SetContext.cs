using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class SetContext : DbContext
    {
        public SetContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Deck> Deck { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<Card>()
            //     .HasOne<Deck>()
            //     .WithMany(x => x.Cards);
        }
    }
}