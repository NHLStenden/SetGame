using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : CrudController<Game>
    {
        public GameController(SetContext setContext) : base(setContext)
        {
        }

        [HttpGet("{id}")]
        public override async Task<Game> GetByIdAsync(int id)
        {
            var entity = await _setContext.Games
                .Include(x => x.Deck)
                    .ThenInclude(x => x.Cards)
                .Include(x => x.Player)
                .SingleAsync(x => x.GameId == id);
            return entity;
        }
        
    }
}