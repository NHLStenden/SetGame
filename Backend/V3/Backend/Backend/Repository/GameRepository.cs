using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(SetContext db) : base(db)
        {
        }

        public async Task<Game> GetByIdWithRelated(int id)
        {
            var entity = await Db.Games
                .Include(x => x.Deck.OrderBy(c => c.Order))
                    .ThenInclude(x => x.Card)
                // .Include(x => x.CardsOnTable.OrderBy(c => c.Order))
                //     .ThenInclude(x => x.Card)
                .Include(x => x.Player)
                .SingleAsync(x => x.Id == id);
            return entity;
        }
    }
}