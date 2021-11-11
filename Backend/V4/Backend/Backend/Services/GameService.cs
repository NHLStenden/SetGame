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
        private readonly ISetService _setService;

        public GameService( IDeckService deckService, 
                            IGameRepository gameRepository, 
                            IPlayerRepository playerRepository, 
                            ISetService setService)
        {
            _deckService = deckService;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _setService = setService;
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

        public async Task<IList<Card>> DrawCardsFromDeck(int gameId, int numberOfCards)
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

            
            game.Deck.Complexity = _setService.CalculateComplexity(game.CardsOnTable.Select(x => x.Card));

            var success = await _gameRepository.UpdateAsync(game);
            if (!success)
                throw new ArgumentException();

            var result = deckCards.Select(x => x.Card).ToList();
            

            
            return result;
        }


        public Task<IList<Card>> GetCardsOnTable(int gameId)
        {
            return _gameRepository.GetCardsOnTable(gameId);
        }

        public async Task<IList<Card>> GetCardsOnTable(int gameId, int[] cardIds)
        {
            return await _gameRepository.GetCardsOnTable(gameId, cardIds);
        }

        public async Task<IList<IList<Card>>> GetAllSetsOnTable(int gameId)
        {
            var cardsOnTable = await _gameRepository.GetCardsOnTable(gameId);
            return _setService.FindAllSets(cardsOnTable);
        }

        public async Task<bool> SubmitSet(int gameId, int[] cardIds)
        {
            var checkSet = await CheckSet(gameId, cardIds);
            
            //Todo: return NoContent
            if (!checkSet.CorrectSet) return false;
            
            
            var game = await _gameRepository.GetByIdWithRelated(gameId);
            //removes card from cardOnTable 
            game.CardsOnTable = game.CardsOnTable.Where(x => !cardIds.Contains(x.CardId)).ToList();
            var success = await _gameRepository.UpdateAsync(game);
            
            //Todo: return new cards instead of boolean 
            return success;
        }

        public async Task<int> CalculateComplexityForCardsOnTable(int gameId)
        {
            var game = await _gameRepository.GetByIdWithRelated(gameId);
            if (!game.CardsOnTable.Any() && game.CardsOnTable.Count < 3)
                return -1;

            var cardsOnTable = game.CardsOnTable.Select(x => x.Card).ToList();

            var complexity = _setService.FindAllSets(cardsOnTable).Count;
            return complexity;
        }

        public async Task<SetResult> CheckSet(int gameId, int[] cardIds)
        {
            if (cardIds.Length != 3)
            {
                throw new ArgumentException();
            }

            var cards = await _gameRepository.GetCardsOnTable(gameId, cardIds);
            if (!cards.Any())
            {
                throw new Exception("No Cards Table");
            }

            var setResult = _setService.Check(cards);

            return setResult;
        }

        // private static bool CheckIfCardsArePlayed(int[] cardIds, Game game)
        // {
        //     var possibleCardsOnBoard = game.Deck.Cards.ToList().GetRange(0, game.CardIndex);
        //     bool validIds = cardIds.All(cardId => possibleCardsOnBoard.Any(x => x.Id == cardId));
        //     return validIds;
        // }
    }
}