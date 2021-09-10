using System.Collections.Generic;
using Backend.Models;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Backend.Controllers
{
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private MySqlConnection GetConnection() => 
            new MySqlConnection("Server=localhost;Database=SetGame;Uid=root;Pwd=Test@1234!;");

        [HttpGet("[action]")]
        public IEnumerable<Card> Get()
        {
            using var conn = GetConnection();
            return conn.GetAll<Card>();
        }
        
        [HttpGet("[action]/{id}")]
        public IActionResult GetById(int id)
        {
            using var conn = new MySqlConnection("Server=localhost;Database=SetGame;Uid=root;Pwd=Test@1234!;");

            var player = conn.Get<Card>(id);
            if (player == null)
                return NotFound();
            return Ok(player);
        }

        [HttpGet("[action]")]
        public IActionResult Create(Shape shape, Fill fill, Color color, int nrOfShapes)
        {
            using var conn = GetConnection();
            var player = new Card()
            {
                Shape = shape,
                Fill = fill,
                Color = color,
                NrOfShapes = nrOfShapes
            };
            var playerId = conn.Insert(player);
            return CreatedAtAction(nameof(GetById), new { id = playerId }, player);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = GetConnection();
            bool success = conn.Delete<Card>(new Card() { CardId = id });
            return success ? Ok() : NotFound();
        }

        [HttpGet("[action]")]
        public ActionResult<Player> Update(Card player)
        {
            using var conn = GetConnection();
            bool updatedPlayer = conn.Update(player);
            return updatedPlayer ? Ok(player) : NotFound();
        }
        
    }
}