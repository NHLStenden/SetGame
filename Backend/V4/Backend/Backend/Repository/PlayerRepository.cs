using Backend.Models;

namespace Backend.Repository
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(SetContext db) : base(db)
        {
        }
    }
}