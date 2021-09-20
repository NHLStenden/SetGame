using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using MoreLinq.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Backend.Services
{
    public abstract class SeedService : ISeedService
    {
        public abstract string GetDataPath();

        public void Seed(SetContext db, IGameService gameService, IPlayerRepository playerRepository)
        {
            var cards = GenerateCards();
            // db.Cards.AddRange(cards);

            var randomNumbers = Enumerable.Range(1, 81)
                .Shuffle(new Random(1024));

            var game = new Game()
            {
                CardIndex = 0,
                Player = new Player()
                {
                    Name = "Joris"
                }
            };

            game.Deck = new List<DeckGameCard>(81);
            for (int i = 0; i < 81; i++)
            {
                game.Deck.Add(new DeckGameCard()
                {
                    Card = cards[i],
                    Order = i
                });
            }

            db.Games.Add(game);
            db.SaveChanges();
        }


        public List<Card> GenerateCards()
        {
            List<Card> cards = new List<Card>(81);

            foreach (var shape in Enum.GetValues<Shape>())
            {
                foreach (var fill in Enum.GetValues<Fill>())
                {
                    foreach (var color in Enum.GetValues<Color>())
                    {
                        for (int shapeIdx = 1; shapeIdx <= 3; shapeIdx++)
                        {
                            var card = new Card()
                            {
                                Shape = shape,
                                Fill = fill,
                                Color = color,
                                NrOfShapes = shapeIdx,
                            };

                            cards.Add(card);
                        }
                    }
                }
            }

            return cards;
        }
        
        public static T YamlToObject<T>(string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var result = deserializer.Deserialize<T>(yaml);
            return result;
        }

        public static string ConvertToYaml<T>(T obj)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yaml = serializer.Serialize(obj);
            return yaml;
        }
    }
}