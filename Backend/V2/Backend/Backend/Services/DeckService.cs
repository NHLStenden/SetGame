using System;
using System.Collections.Generic;
using Backend.Models;
using Backend.Utils;

namespace Backend.Services
{
    public class DeckService
    {
        private static int dekcId = 1;
        private static int cardId = 1;
        
        public Deck CreateDeck()
        {
            var deck = new Deck()
            {
                DeckId = dekcId++,
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
                card.CardId = cardId++;
            }
            
            return deck;
        }
    }
}