using System;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("[action]")]
        public Card[] DrawCards(int gameId, int numberOfCards)
        {
            if (numberOfCards <= 0)
                throw new ArgumentException();
            
            return _gameService.DrawCardsFromDeck(gameId, numberOfCards);
        }
    }
}