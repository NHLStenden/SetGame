using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class CrudController<T> : ControllerBase where T : class, IEntity, new()
    {
        protected readonly IGenericRepository<T> Repository;

        public CrudController(IGenericRepository<T> repository)
        {
            Repository = repository;
        }
        
        [HttpGet()]
        public virtual async Task<IEnumerable<T>> GetAsync()
        {
            var result = await Repository.GetAllAsync();
            return result;
        }
        
        [HttpGet("{id}")]
        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await Repository.GetByIdAsync(id);
            return entity;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var success = await Repository.AddAsync(entity);
            if (!success)
            {
                return BadRequest();
            }

            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await Repository.DeleteAsync(id);

            return success ? 
                Ok() : 
                NotFound();
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<T>> UpdateAsync(int id, T entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (id != entity.Id)
            {
                return BadRequest();
            }

            var success = await Repository.UpdateAsync(entity);

            return success ? entity : NotFound();
        }
    }
}