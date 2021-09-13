using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : CrudController<Game>
    {
        private readonly IGameRepository _gameRepository;

        public GameController(IGameRepository gameRepository) : base(gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpGet("{id}")]
        public override async Task<Game> GetByIdAsync(int id)
        {
            return await _gameRepository.GetByIdWithRelated(id);
        }
        
    }
}