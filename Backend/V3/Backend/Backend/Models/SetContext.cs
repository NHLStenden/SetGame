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
        // public DbSet<GameCard> GameCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<DeckGameCard>()
                .HasKey(x => new {x.GameId, x.CardId});

            modelBuilder.Entity<DeckGameCard>()
                .HasOne(gc => gc.Card)
                .WithMany(b => b.Deck)
                .HasForeignKey(gc => gc.CardId);

            modelBuilder.Entity<DeckGameCard>()
                .HasOne(gc => gc.Game)
                .WithMany(g => g.Deck)
                .HasForeignKey(gc => gc.GameId);
            
            
            modelBuilder.Entity<TableGameCard>()
                .HasKey(x => new {x.GameId, x.CardId});

            modelBuilder.Entity<TableGameCard>()
                .HasOne(gc => gc.Card)
                .WithMany(b => b.OnTable)
                .HasForeignKey(bc => bc.CardId);

            modelBuilder.Entity<TableGameCard>()
                .HasOne(gc => gc.Game)
                .WithMany(g => g.OnTable)
                .HasForeignKey(gc => gc.GameId);
        }
    }
}