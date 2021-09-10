using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Backend.Repository
{
    public interface IGenericRepository<T> where T: class, new()
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
        Task<int> AddAsync(T t);
        Task<bool> UpdateAsync(T t);
        
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        private readonly IConfiguration _configuration;

        private MySqlConnection GetConnection() => 
            new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        public GenericRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public Task<IEnumerable<T>> GetAllAsync()
        {
            var result = GetConnection().GetAllAsync<T>();
            return result;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var t = new T();
            
            var keyProperty = typeof(T)
                .GetProperties()
                .Single(info => info.IsDefined(typeof(KeyAttribute), false));
            
            keyProperty.SetValue(t, id);
            
            using var conn = GetConnection();
            var success = conn.DeleteAsync<T>(t);
            return success;
        }

        public Task<T> GetAsync(int id)
        {
            using var conn = GetConnection();

            var player = conn.GetAsync<T>(id);
            return player;
        }

        public Task<bool> UpdateAsync(T t)
        {
            using var conn = GetConnection();
            var updatedEntity = conn.UpdateAsync(t);
            return updatedEntity;
        }

        public Task<int> AddAsync(T t)
        {
            using var conn = GetConnection();
  
            var id = conn.InsertAsync(t);
            return id;
        }
    }
}