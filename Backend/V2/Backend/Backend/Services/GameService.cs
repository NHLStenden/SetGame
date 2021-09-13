using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Dapper;
using Dapper.Logging;

namespace Backend.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IDeckService _deckService;
        private readonly IDeckRepository _deckRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IDbConnectionFactory _factory;

        public GameService(IGameRepository gameRepository, IDeckService deckService, IDeckRepository deckRepository, ICardRepository cardRepository, IDbConnectionFactory factory)
        {
            _gameRepository = gameRepository;
            _deckService = deckService;
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
            _factory = factory;
        }
        
        // public Game StartNewGame(int playerId)
        // {
        //     var deck = new DeckService().CreateDeck();
        //     var player = new PlayerService().GetById(playerId);
        //     
        //     _db.Games.Add(new Game()
        //     {
        //         GameId = _db.Games.Count + 1,
        //         Deck = deck,
        //         DeckId = deck.DeckId,
        //         Player = player,
        //         PlayerId = player.PlayerId
        //     });
        //
        //     return _db.Games[^1];
        // }

        public async Task<Card[]> DrawCardsFromDeck(int gameId, int numCards)
        {
            Game game = await _gameRepository.GetAsync(gameId);
            //Todo throw deck not found exception or not found expcetion
            
            int endIndex = Math.Min(81, game.CardIndex + numCards);
            var deckCards = game.Deck.Cards[game.CardIndex..endIndex];
            game.CardIndex = endIndex;
            
            return deckCards;
        }
        
        public async Task<int> StartNewGame(int playerId, int? deckId)
        {
            Deck deck = null;
            if (deckId.HasValue)
            {
                deck = _deckService.GetById(deckId);
            }
            else
            {
                deck = await _deckService.CreateDeck();
                //deckId = await _deckRepository.AddAsync(deck);
            }
            
            Game game = new Game()
            {
                DeckId = deckId.Value
            };
            game.PlayerId = playerId;

            
            
            //await _gameRepository.AddAsync(game);
            
            return game.GameId;
        }
    }
}