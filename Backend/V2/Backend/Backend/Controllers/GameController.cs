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
                throw new ArgumentOutOfRangeException($"numOfCards: {numberOfCards} should be > 0");
            
            return _gameService.DrawCardsFromDeck(gameId, numberOfCards);
        }

        [HttpGet("[action]")]
        public int StartGame(int playerId)
        {
            return _gameService.StartNewGame(playerId);
        }
    }
}