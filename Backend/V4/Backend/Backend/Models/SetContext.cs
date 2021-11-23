using Backend.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class SetContext : IdentityDbContext<Player, IdentityRole<int>, int>
    {
        public SetContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Deck> Deck { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //seed database with roll data
            builder.ApplyConfiguration(new RollConfiguration());
            
            builder.Entity<CardDeck>()
                .HasKey(cardDeck => new {cardDeck.DeckId, cardDeck.CardId});
            
            builder.Entity<CardOnTable>()
                .HasKey(cardOnTable => new {cardOnTable.GameId, cardOnTable.CardId});
            
            //to prevent a bug in mysql: https://stackoverflow.com/questions/48678495/net-core-2-0-with-mysql-specified-key-was-too-long-max-key-length-is-3072-byt
                int maxKeySize = 255;
                
                builder.Entity<IdentityUserLogin<int>>()
                    .Property(u => u.LoginProvider)
                    .HasMaxLength(maxKeySize);
                builder.Entity<IdentityUserLogin<int>>()
                    .Property(u => u.ProviderKey)
                    .HasMaxLength(maxKeySize);

                // builder.Entity<IdentityUserRole<string>>()
                //     .Property(ur => ur.UserId)
                //     .HasMaxLength(maxKeySize);
                //
                // builder.Entity<IdentityUserRole<string>>()
                //     .Property(ur => ur.RoleId)
                //     .HasMaxLength(maxKeySize);
                //
                builder.Entity<IdentityUserToken<int>>()
                    .Property(ut => ut.LoginProvider)
                    .HasMaxLength(maxKeySize);
                
                builder.Entity<IdentityUserToken<int>>()
                    .Property(ut => ut.UserId)
                    .HasMaxLength(maxKeySize);
                
                builder.Entity<IdentityUserToken<int>>()
                    .Property(ut => ut.Name)
                    .HasMaxLength(maxKeySize);
        }
    }
}