using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected readonly SetContext Db;

        public GenericRepository(SetContext db)
        {
            Db = db;
        }
        
        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await Db.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await Db.Set<T>().AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entityToDelete = await GetByIdAsync(id);
            Db.Set<T>().Remove(entityToDelete);
            int numRowEffected = await Db.SaveChangesAsync();
            return numRowEffected > 0;
        }

        public async Task<bool> AddAsync(T entity)
        {
            await Db.Set<T>().AddAsync(entity);
            var numRowsEffected = await Db.SaveChangesAsync();
            return numRowsEffected > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            Db.Set<T>().Update(entity);
            var numRowsEffected = await Db.SaveChangesAsync();
            return numRowsEffected > 0;
        }
    }
}