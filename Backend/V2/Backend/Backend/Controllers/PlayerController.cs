using Backend.Models;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private MySqlConnection GetConnection() => 
             new MySqlConnection("Server=localhost;Database=SetGame;Uid=root;Pwd=Test@1234!;");

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
            using var conn = GetConnection();
            var player = new Player()
            {
                Name = name
            };
            var playerId = conn.Insert(player);
            return CreatedAtAction(nameof(GetById), new { id = playerId }, player);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = GetConnection();
            bool sucess = conn.Delete<Player>(new Player() { PlayerId = id });
            return sucess ? Ok() : NotFound();
        }

        [HttpGet("[action]")]
        public ActionResult<Player> Update(Player player)
        {
            using var conn = GetConnection();
            bool updatedPlayer = conn.Update(player);
            return updatedPlayer ? Ok(player) : NotFound();
        }
        
    }
}