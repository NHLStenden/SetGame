using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Backend.Utils;
using MoreLinq;

namespace Backend.Services
{
    public class DeckService : IDeckService
    {
        private int NUMBER_OF_CARDS = 81;
        
        private readonly ICardRepository _cardRepository;

        public DeckService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }
        
        public async Task<List<Card>> CreateCards()
        {
            var cards = new List<Card>(NUMBER_OF_CARDS);
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
                            await _cardRepository.AddAsync(card);
                        }
                    }
                }
            }

            return cards;
        }
        
        public async Task<Deck> CreateDeck()
        {
            var cards = await _cardRepository.GetAllAsync();

            if (!cards.Any())
            {
                cards = await CreateCards();
            }
            
            var randomIndexes = Enumerable
                .Range(0, NUMBER_OF_CARDS)
                .Shuffle()
                .ToArray();

            var deck = new Deck()
            {
                Complexity = -1,
                Cards = new List<CardDeck>()
            };
            
            for (int i = 0; i < NUMBER_OF_CARDS; i++)
            {
                var cardDeck = new CardDeck()
                {
                    CardId = cards.ElementAt(randomIndexes[i]).Id,
                    Deck = deck,
                    Order = i
                };
                deck.Cards.Add(cardDeck);
            }
            
            return deck;
        }
    }
}