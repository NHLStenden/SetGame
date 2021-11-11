using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameRepository _gameRepository;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GameController(IUnitOfWork unitOfWork, IGameRepository gameRepository, IGameService gameService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gameRepository = gameRepository;
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<GameDto> GetByIdAsync(int id)
        {
            var game = await _gameRepository.GetByIdWithRelated(id);
            
            var gameDto = _mapper.Map<GameDto>(game);
            
            return gameDto;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _gameRepository.DeleteAsync(id);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("[action]/{playerId:int}")]
        public async Task<GameDto> StartNewGame(int playerId)
        {
            var game = await _gameService.StartNewGame(playerId);

            await _unitOfWork.SaveChangesAsync();
            
            var gameDto = _mapper.Map<GameDto>(game);

            return gameDto;
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