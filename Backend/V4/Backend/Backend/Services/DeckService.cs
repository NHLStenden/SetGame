using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using MoreLinq;

namespace Backend.Services
{
    public class DeckService : IDeckService
    {
        private int NUMBER_OF_CARDS = 81;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICardRepository _cardRepository;
        private readonly ISetService _setService;

        public DeckService(IUnitOfWork unitOfWork, ICardRepository cardRepository, ISetService setService)
        {
            _unitOfWork = unitOfWork;
            _cardRepository = cardRepository;
            _setService = setService;
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

            await _unitOfWork.SaveChangesAsync();
            return cards;
        }
        
        public async Task<Deck> CreateDeck()
        {
            var cards = (await _cardRepository.GetAllAsync()).ToList();

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
                
                Cards = new List<CardDeck>()
            };

            var cardsToCalculateComplexity = new List<Card>();
            for (int i = 0; i < NUMBER_OF_CARDS; i++)
            {
                var cardAtRandomIndex = cards.ElementAt(randomIndexes[i]);
                var cardDeck = new CardDeck()
                {
                    CardId = cardAtRandomIndex.Id,
                    Deck = deck,
                    Order = i
                };
                deck.Cards.Add(cardDeck);

                if (i < 12)
                {
                    cardsToCalculateComplexity.Add(cardAtRandomIndex);
                }
            }

            deck.Complexity = _setService.CalculateComplexity(cardsToCalculateComplexity);
            await _unitOfWork.SaveChangesAsync();
            return deck;
        }
    }
}