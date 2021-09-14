using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repository
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<Game> GetByIdWithRelated(int id);
    }
}