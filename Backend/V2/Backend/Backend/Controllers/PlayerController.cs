using Backend.Models;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Backend.Controllers
{
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        [HttpGet("[action]/{id}")]
        public IActionResult GetById(int id)
        {
            using var conn = new MySqlConnection("Server=localhost;Database=SetGame;Uid=root;Pwd=Test@1234!;");

            var player = conn.Get<Player>(id);
            if (player == null)
                return NotFound();
            return Ok(player);
        }

        [HttpGet("[action]")]
        public IActionResult Create(string name)
        {
            using var conn = new MySqlConnection("Server=localhost;Database=SetGame;Uid=root;Pwd=Test@1234!;");

            var player = new Player()
            {
                Name = name
            };
            var playerId = conn.Insert(player);
            
            return CreatedAtAction(nameof(GetById), new { id = playerId }, player);
        }
        
    }
}