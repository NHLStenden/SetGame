using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : CrudController<Game>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGameService _gameService;

        public GameController(IGameRepository gameRepository, IGameService gameService) : base(gameRepository)
        {
            _gameRepository = gameRepository;
            _gameService = gameService;
        }

        [HttpGet("{id}")]
        public override async Task<Game> GetByIdAsync(int id)
        {
            return await _gameRepository.GetByIdWithRelated(id);;
        }

        [HttpGet("[action]/{playerId:int}")]
        public async Task<Game> StartGame(int playerId)
        {
            return await _gameService.StartNewGame(playerId);
        }

        [HttpGet("[action]/{gameId:int}")]
        public Task<List<Card>> DrawCards(int gameId, int numberOfCards)
        {
            return _gameService.DrawCardsFromDeck(gameId, numberOfCards);
        }
        
  
     }
}