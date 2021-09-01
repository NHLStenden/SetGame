using System;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        [HttpGet("[action]")]
        public Card[] DrawCards(int gameId, int numCards)
        {
            if (numCards <= 0)
                throw new ArgumentException();
            
            return new GameService().DrawCardsFromDeck(gameId, numCards);
        }
    }
}