using System.Collections.Generic;
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

        public async Task<Game> GetByIdWithRelated(int gameId)
        {
            var entity = await Db.Games
                .Where(x => x.Id == gameId)
                .Include(x => x.Deck)
                    .ThenInclude(x => x.Cards.OrderBy(w => w.Order))
                .ThenInclude(x => x.Card)
                .Include(x => x.Player)
                .Include(x => x.CardsOnTable.OrderBy(c => c.Order))
                .SingleAsync(x => x.Id == gameId);
            return entity;
        }

        public async Task<IList<Card>> GetCardsOnTable(int gameId)
        {
            return await 
                Db.Games
                    .Where(x => x.Id == gameId)
                    .SelectMany(x => x.CardsOnTable.OrderBy(w => w.Order))
                    .Select(x => x.Card).ToListAsync();
        }

        public async Task<IList<Card>> GetCardsOnTable(int gameId, int[] cardIds)
        {
            return await Db.Games
                .Where(x => x.Id == gameId)
                .SelectMany(x => x.CardsOnTable.Where(w => cardIds.Contains(w.CardId))
                .OrderBy(o => o.Order)
                .Select(w => w.Card))
                .ToListAsync();
        }
    }
}