using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using Backend.Utils;

namespace Backend.Services
{
    public interface IDeckService
    {
        Deck CreateDeck();
        Deck GetById(int? deckId);
    }

    public class DeckService : IDeckService
    {
        public Deck CreateDeck()
        {
            var deck = new Deck()
            {
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

            return deck;
        }

        public Deck GetById(int? deckId)
        {
            throw new NotImplementedException();
        }
    }
}