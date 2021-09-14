using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services
{
    public class GameService : IGameService
    {
        private readonly IDeckService _deckService;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;

        public GameService(IDeckService deckService, IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _deckService = deckService;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }
        
        public async Task<Game> StartNewGame(int playerId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            if (player == null)
            {
                throw new ArgumentException("Player doesn't exists");
            }

            var game = new Game()
            {
                Deck = _deckService.CreateDeck(),
                PlayerId = playerId
            };

            var success = await _gameRepository.AddAsync(game);

            if (success)
            {
                return game;
            }
            else
            {
                throw new Exception();
            }
        }
        
        public async Task<List<Card>> DrawCardsFromDeck(int gameId, int numberOfCards)
        {
            Game game = await _gameRepository.GetByIdWithRelated(gameId);
            if (game == null)
                throw new ArgumentException("game doesn't exists");
            
            int endIndex = Math.Min(81, game.CardIndex + numberOfCards);
            
            var deckCards = game.Deck.Cards.ToList().GetRange(game.CardIndex,endIndex);
            game.CardIndex = endIndex;

            var success = await _gameRepository.UpdateAsync(game);
            if (!success)
                throw new ArgumentException();

            return deckCards;
        }
    }
}