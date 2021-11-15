using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class SetContext : IdentityDbContext<ApiUser>
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

            modelBuilder.Entity<CardDeck>()
                .HasKey(cardDeck => new {cardDeck.DeckId, cardDeck.CardId});


            modelBuilder.Entity<CardOnTable>()
                .HasKey(cardOnTable => new {cardOnTable.GameId, cardOnTable.CardId});
        }
    }
}