using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repository
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<Game> GetByIdWithRelated(int id);
        Task<List<Card>> GetCardsOnTable(int gameId);
        Task<List<Card>> GetCardsOnTable(int gameId, int[] cardIds);
    }
}