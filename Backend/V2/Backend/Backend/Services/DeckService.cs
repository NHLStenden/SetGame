using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using Backend.Utils;

namespace Backend.Services
{
    public class DeckService
    {
        private readonly SetContext _db;

        public DeckService(SetContext db)
        {
            _db = db;
        }
        
        public Deck CreateDeck()
        {
            var deck = new Deck()
            {
                DeckId = _db.Games.Max(x => x.DeckId) + 1,
                Cards = new Card[81]
            };

            int cardIdx = 0;
            foreach (var shape in Enum.GetValues<Shape>())
            {
                foreach (var fill in Enum.GetValues<Fill>())
                {
                    foreach (var color in Enum.GetValues<Color>())
                    {
                        for (int shapeIdx = 1; shapeIdx <= 3; shapeIdx++)
                        {
                            deck.Cards[cardIdx++] = new Card()
                            {
                                Shape = shape,
                                Fill = fill,
                                Color = color,
                                NrOfShapes = shapeIdx
                            };
                        }
                    }
                }
            }

            deck.Cards.Shuffle();
            
            foreach (var card in deck.Cards)
            {
                card.CardId = _db.Games.Max(x => x.Deck.Cards.Max(x => x.CardId)) + 1;
            }
            
            return deck;
        }

        public Deck GetById(int? deckId)
        {
            throw new NotImplementedException();
        }
    }
}