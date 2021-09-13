using System.Collections.Generic;
using System.Linq;

namespace Backend.Models
{
    public class DbInitializer
    {
        public static void Initialize(SetContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            var card = new Card()
            {
                Color = Color.Green,
                Fill = Fill.Hollow,
                Shape = Shape.Diamond,
                NrOfShapes = 3,
                DeckIndex = 0
            };

            var deck = new Deck()
            {
                Name = "Joris first Deck",
                Cards = new List<Card>()
                {
                    card
                }
            };

            var game = new Game()
            {
                CardIndex = 0,
                Deck = deck,
                Player = new Player()
                {
                    Name = "Joris"
                }
            };
            
            context.Games.Add(game);
            context.SaveChanges();
        }
    }
}