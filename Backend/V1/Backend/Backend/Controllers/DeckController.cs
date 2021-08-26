using System;
using System.Collections.Generic;
using Backend.Models;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class DeckController : ControllerBase
    {
        private static List<Deck> Decks { get; set; } = new List<Deck>();

        private List<Card> GenerateDeckCards()
        {
            List<Card> deck = new List<Card>();
            foreach (var shape in Enum.GetValues<Shape>())
            {
                foreach (var fill in Enum.GetValues<Fill>())
                {
                    foreach (var color in Enum.GetValues<Color>())
                    {
                        for (int shapeIdx = 1; shapeIdx <= 3; shapeIdx++)
                        {
                            deck.Add(new Card()
                            {
                                Shape = shape,
                                Fill = fill,
                                Color = color,
                                NrOfShapes = shapeIdx
                            });
                        }
                    }
                }
            }

            deck.Shuffle();
            return deck;
        }
        
        [HttpGet("[action]")]
        public Deck GetNewDeck()
        {
            var deck = new Deck()
            {
                DeckId = Decks.Count + 1,
                Cards = GenerateDeckCards()
            };
            Decks.Add(deck);
            return deck;
        }

        [HttpGet("[action]")]
        public Deck GetDeckById(int deckId)
        {
            var deck = Decks.Find(x => x.DeckId == deckId);
            return deck;
        }

        [HttpGet("[action]")]
        public Card GetNextCardFromDeck(int deckId)
        {
            var deck = GetDeckById(deckId);
            return deck?.DrawCard();
        }
    }
}