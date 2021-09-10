using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class CrudController<T> : ControllerBase where T : class, new()
    {
        private readonly IGenericRepository<T> _repository;

        public CrudController(IGenericRepository<T> repository)
        {
            _repository = repository;
        }
        
        [HttpGet("[action]")]
        public async Task<IEnumerable<T>> GetAsync()
        {
            var result = await _repository.GetAllAsync();
            return result;
        }
        
        [HttpGet("[action]/{id}")]
        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _repository.GetAsync(id);
            return entity;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateAsync(T entity)
        {
            var id = await _repository.AddAsync(entity);
            string getByIddMethodName = nameof(GetByIdAsync);
            return CreatedAtAction(getByIddMethodName, new { id }, entity);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool success = await _repository.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<T>> UpdateAsync(T entity)
        {
            var success = await _repository.UpdateAsync(entity);
            return success ? Ok() : NotFound();
        }
    }
}