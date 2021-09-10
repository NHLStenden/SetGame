using System.Collections.Generic;
using System.Threading.Tasks;

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
}