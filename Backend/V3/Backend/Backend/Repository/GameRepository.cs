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

        public async Task<Game> GetByIdWithRelated(int id)
        {
            var entity = await Db.Games
                .Include(x => x.Deck)
                    .ThenInclude(x => x.Cards.OrderBy(w => w.Order))
                .ThenInclude(x => x.Card)
                .Include(x => x.Player)
                .Include(x => x.CardsOnTable)
                .SingleAsync(x => x.Id == id);
            return entity;
        }

        public async Task<List<Card>> GetCardsOnTable(int gameId)
        {
            return await Db.Games.SelectMany(x => x.CardsOnTable.Select(w => w.Card)).ToListAsync();
        }

        public async Task<List<Card>> GetCardsOnTable(int gameId, int[] cardIds)
        {
            return await Db.Games.SelectMany(x => 
                x.CardsOnTable.Where(w => cardIds.Contains(w.CardId)).Select(w => w.Card)).ToListAsync();
        }
    }
}