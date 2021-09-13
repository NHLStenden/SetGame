using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public class CrudController<T> : ControllerBase where T : class, new()
    {
        protected readonly SetContext _setContext;
        
        public CrudController(SetContext setContext)
        {
            _setContext = setContext;
        }
        
        [HttpGet()]
        public async Task<IEnumerable<T>> GetAsync()
        {
            var result = await _setContext.Set<T>().AsNoTracking().ToListAsync();
            return result;
        }
        
        [HttpGet("{id}")]
        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await _setContext.Set<T>().FindAsync(id);
            return entity;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(T entity)
        {
            await _setContext.Set<T>().AddAsync(entity);
            var numRowsEffected = await _setContext.SaveChangesAsync();
            if (numRowsEffected <= 0)
            {
                return BadRequest();
            }
            
            string getByIddMethodName = nameof(GetByIdAsync);
            return CreatedAtAction(getByIddMethodName, new { id = GetKey(entity) }, entity);
            
            int GetKey<T>(T entity)
            {
                var keyName = _setContext.Model.FindEntityType(typeof (T)).FindPrimaryKey().Properties
                    .Select(x => x.Name).Single();

                return (int)entity.GetType().GetProperty(keyName).GetValue(entity, null);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var entityToDelete = await GetByIdAsync(id);
            _setContext.Set<T>().Remove(entityToDelete);
            int success = await _setContext.SaveChangesAsync();
            return success > 0 ? Ok() : NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<T>> UpdateAsync(int id, T entity)
        {
            _setContext.Set<T>().Update(entity);
            var numRowsEffected = await _setContext.SaveChangesAsync();
            return numRowsEffected > 0 ? Ok() : NotFound();
        }
    }
}