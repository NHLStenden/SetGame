using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : CrudController<Player>
    {
        public PlayerController(IPlayerRepository repository) : base(repository)
        { }

        [HttpPost()]
        public override async Task<IActionResult> CreateAsync(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var success = await Repository.AddAsync(player);
            if (!success)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = player.Id }, player);

            
        }
    }
}