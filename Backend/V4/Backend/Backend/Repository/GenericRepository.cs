using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected readonly SetContext Db;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(SetContext db)
        {
            Db = db;
            DbSet = Db.Set<T>();
        }
        
        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await DbSet.FindAsync(id);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await DbSet.AsNoTracking().ToListAsync();
            return entities;
        }

        public async Task DeleteAsync(int id)
        {
            var entityToDelete = await GetByIdAsync(id);
            DbSet.Remove(entityToDelete);
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            // DbSet.Attach(entity); //not sure if this is really needed
            DbSet.Update(entity);
        }
    }
}