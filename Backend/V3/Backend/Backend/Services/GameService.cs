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

            //print to yaml
            var success = await _gameRepository.AddAsync(game);
            
            string yaml = SeedService.ConvertToYaml(game);

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

        public async Task<SetResult> CheckSet(int gameId, int[] cardIds)
        {
            throw new NotImplementedException();
            // if (cardIds.Length != 3)
            // {
            //     throw new ArgumentException();
            // }
            //
            // var game = await _gameRepository.GetByIdWithRelated(gameId);
            //
            // bool validIds = CheckIfCardsArePlayed(cardIds, game);
            // if (!validIds)
            // {
            //     throw new AggregateException("Invalid cardid, card not played yet");
            // }
            //
            // //Todo: refactor to GetCurrentCards();
            // var cards = game.Deck.Cards.Where(x => cardIds.Any(w => w == x.Id)).ToList();
            //
            // var firstCard = cards.First();
            // bool colorSame = cards.All(x => x.Color == firstCard.Color);
            // bool shapeSame = cards.All(x => x.Shape == firstCard.Shape);
            // bool fillSame = cards.All(x => x.Fill == firstCard.Fill);
            //
            // bool colorDifferent = cards.DistinctBy(x => x.Color).Count() == 3;
            // bool shapeDifferent = cards.DistinctBy(x => x.Shape).Count() == 3;
            // bool fillDifferent = cards.DistinctBy(x => x.Fill).Count() == 3;
            //
            // var result = new SetResult()
            // {
            //     ColorsCorrect = colorSame || colorDifferent,
            //     ColorSame = colorSame,
            //     ColorDifferent = colorDifferent,
            //     
            //     ShapeCorrect = shapeSame || shapeDifferent,
            //     ShapeSame = shapeSame,
            //     ShapeDifferent = shapeDifferent,
            //     
            //     FillCorrect = fillSame || fillDifferent,
            //     FillSame = fillSame,
            //     FillDifferent = fillDifferent
            // };
            //
            // result.CorrectSet = result.ColorsCorrect && result.ShapeCorrect && result.FillCorrect;
            //
            // return result;
        }

        // private static bool CheckIfCardsArePlayed(int[] cardIds, Game game)
        // {
        //     var possibleCardsOnBoard = game.Deck.Cards.ToList().GetRange(0, game.CardIndex);
        //     bool validIds = cardIds.All(cardId => possibleCardsOnBoard.Any(x => x.Id == cardId));
        //     return validIds;
        // }
    }

    public class SetResult
    {
        public bool CorrectSet { get; set; }
        
        public bool ColorsCorrect { get; set; }
        public bool ShapeCorrect { get; set; }
        public bool FillCorrect { get; set; }
        
        public bool ColorSame { get; set; }
        public bool ColorDifferent { get; set; }
        public bool ShapeSame { get; set; }
        public bool ShapeDifferent { get; set; }
        public bool FillSame { get; set; }
        public bool FillDifferent { get; set; }
        
    }
}