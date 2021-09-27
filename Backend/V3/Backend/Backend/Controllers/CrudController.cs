using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class CrudController<T> : ControllerBase where T : class, IEntity, new()
    {
        private readonly IGenericRepository<T> _repository;

        public CrudController(IGenericRepository<T> repository)
        {
            _repository = repository;
        }
        
        [HttpGet()]
        public async Task<IEnumerable<T>> GetAsync()
        {
            var result = await _repository.GetAllAsync();
            return result;
        }
        
        [HttpGet("{id}")]
        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(T entity)
        {
            var success = await _repository.AddAsync(entity);
            if (!success)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);

            return success ? Ok() : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<T>> UpdateAsync(int id, T entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }

            var success = await _repository.UpdateAsync(entity);

            return success ? entity : NotFound();
        }
    }
}