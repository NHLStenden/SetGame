using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Services
{
    public class DeckService : IDeckService
    {
        public Deck CreateDeck()
        {
            var deck = new Deck();
            deck.Cards = new List<Card>(81);
            
            
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

                            deck.Cards.Add(card);
                        }
                    }
                }
            }

            deck.Cards.Shuffle();
            
            int cardIdx = 0;
            foreach (var card in deck.Cards)
            {
                card.CardIndex = cardIdx++;
            }
            
            return deck;
        }

        public Deck GetById(int? deckId)
        {
            throw new NotImplementedException();
        }
    }
}