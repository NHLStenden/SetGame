using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;


        public GameController(IGameRepository gameRepository, IGameService gameService, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<GameViewModel> GetByIdAsync(int id)
        {
            var game = await _gameRepository.GetByIdWithRelated(id);
            var gameViewModel = _mapper.Map<GameViewModel>(game);
            return gameViewModel;
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _gameRepository.DeleteAsync(id);

            return success ? 
                Ok() : 
                NotFound();
        }

        [HttpGet("[action]/{playerId:int}")]
        public async Task<Game> StartNewGame(int playerId)
        {
            return await _gameService.StartNewGame(playerId);
        }

        [HttpGet("[action]/{gameId:int}")]
        public Task<IList<Card>> DrawCards(int gameId, int numberOfCards)
        {
            return _gameService.DrawCardsFromDeck(gameId, numberOfCards);
        }

        [HttpGet("[action]/{gameId:int}")]
        public Task<IList<Card>> GetCardsOnTable(int gameId)
        {
            return _gameService.GetCardsOnTable(gameId);
        }

        [HttpGet("[action]/{gameId:int}")]
        public Task<SetResult> CheckSet(int gameId, [FromQuery] int[] cardIds)
        {
            return _gameService.CheckSet(gameId, cardIds);
        }

        [HttpGet("[action]/{gameId:int}")]
        public async Task<bool> SubmitSet(int gameId, [FromQuery] int[] cardIds)
        {
            return await _gameService.SubmitSet(gameId, cardIds);
        }

        [HttpGet("[action]/{gameId:int}")]
        public Task<IList<IList<Card>>> GetAllSetsOnTable(int gameId)
        {
            return _gameService.GetAllSetsOnTable(gameId);
        }

        [HttpGet("[action]/{gameId:int}")]
        public Task<int> CalculateComplexityForCardsOnTable(int gameId)
        {
            return _gameService.CalculateComplexityForCardsOnTable(gameId);
        }
     }
}