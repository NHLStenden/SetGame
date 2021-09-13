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
    public class GameController : ControllerBase
    {
        private readonly SetContext _db;

        public GameController(SetContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<List<Game>> Get()
        {
            var result = await _db.Games.AsNoTracking().ToListAsync();
            return result;
        }
        
        [HttpGet("{gameId:int}")]
        public async Task<Game> Get(int gameId)
        {
            return await _db.Games.FindAsync(gameId);
        }
    }
}