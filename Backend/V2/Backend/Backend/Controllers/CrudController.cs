using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Backend.Controllers
{
    public class CrudController<T> : ControllerBase where T : class, new()
    {
        private readonly IConfiguration _configuration;

        private MySqlConnection GetConnection() => 
            new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public CrudController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet("[action]")]
        public IEnumerable<T> Get()
        {
            using var conn = GetConnection();
            var result = conn.GetAll<T>();
            return result;
        }
        
        [HttpGet("[action]/{id}")]
        public IActionResult GetById(int id)
        {
            using var conn = new MySqlConnection("Server=localhost;Database=SetGame;Uid=root;Pwd=Test@1234!;");

            var player = conn.Get<T>(id);
            if (player == null)
                return NotFound();
            return Ok(player);
        }

        [HttpGet("[action]")]
        public IActionResult Create(T entity)
        {
            using var conn = GetConnection();
  
            var id = conn.Insert(entity);
            return CreatedAtAction(nameof(GetById), new { id = id }, entity);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            var t = new T();
            
            var keyProperty = typeof(T)
                .GetProperties()
                .Single(info => info.IsDefined(typeof(KeyAttribute), false));
            
            keyProperty.SetValue(t, id);
            
            using var conn = GetConnection();
            bool success = conn.Delete<T>(t);
            return success ? Ok() : NotFound();
        }

        [HttpGet("[action]")]
        public ActionResult<T> Update(T entity)
        {
            using var conn = GetConnection();
            bool updatedEntity = conn.Update(entity);
            return updatedEntity ? Ok(entity) : NotFound();
        }
    }
}