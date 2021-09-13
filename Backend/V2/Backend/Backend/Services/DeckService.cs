using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Services
{
    public interface IDeckService
    {
        Task<Deck> CreateDeck();
        Deck GetById(int? deckId);
    }

    public class DeckService : IDeckService
    {
        private readonly IDeckRepository _deckRepository;
        private readonly ICardRepository _cardRepository;

        public DeckService(IDeckRepository deckRepository, ICardRepository cardRepository)
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
        }

        public async Task<Deck> CreateDeck()
        {
            var deck = new Deck()
            {
                
            };

            await _deckRepository.AddAsync(deck);
            

            int cardIdx = 0;
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
                                DeckId = deck.DeckId,
                                DeckIndex = cardIdx
                            };

                            await _cardRepository.AddAsync(card);
                            
                            deck.Cards[cardIdx++] = card;
                        }
                    }
                }
            }

            deck.Cards.Shuffle();

            foreach (var card in deck.Cards)
            {
                
            }
            
            return deck;
        }

        public Deck GetById(int? deckId)
        {
            throw new NotImplementedException();
        }
    }
}