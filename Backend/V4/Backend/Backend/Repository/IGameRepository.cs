using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repository
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task<Game> GetByIdWithRelated(int gameId);
        Task<IList<Card>> GetCardsOnTable(int gameId);
        Task<IList<Card>> GetCardsOnTable(int gameId, int[] cardIds);
    }
}