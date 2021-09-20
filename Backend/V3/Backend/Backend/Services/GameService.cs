using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using MoreLinq.Extensions;

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
                Deck = await _deckService.CreateDeck(),
                PlayerId = playerId
            };

            game.CardsOnTable = new List<CardOnTable>();



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

            var deckCards = game.Deck.Cards.ToList().GetRange(game.CardIndex, endIndex - game.CardIndex);
            game.CardIndex = endIndex;

            int order = 0;
            if (game.CardsOnTable.Any())
            {
                order = game.CardsOnTable.Max(x => x.Order) + 1;
            }

            foreach (var deckCard in deckCards)
            {
                game.CardsOnTable.Add(new CardOnTable()
                {
                    Card = deckCard.Card,
                    Order = order
                });
                order++;
            }

            var success = await _gameRepository.UpdateAsync(game);
            if (!success)
                throw new ArgumentException();

            var result = deckCards.Select(x => x.Card).ToList();
            return result;
        }

        public async Task<List<Card>> GetCardsOnTable(int gameId)
        {
            return await _gameRepository.GetCardsOnTable(gameId);
        }

        public async Task<SetResult> CheckSet(int gameId, int[] cardIds)
        {
            if (cardIds.Length != 3)
            {
                throw new ArgumentException();
            }

            var cardsOnTable = await _gameRepository.GetCardsOnTable(gameId);



            throw new NotImplementedException();
        }

        // private static bool CheckIfCardsArePlayed(int[] cardIds, Game game)
        // {
        //     var possibleCardsOnBoard = game.Deck.Cards.ToList().GetRange(0, game.CardIndex);
        //     bool validIds = cardIds.All(cardId => possibleCardsOnBoard.Any(x => x.Id == cardId));
        //     return validIds;
        // }
    }
}