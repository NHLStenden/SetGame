using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper.Logging;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Backend.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GenericRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private IDbConnection GetConnection() => _connectionFactory.CreateConnection();

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
            var success = conn.DeleteAsync(t);
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