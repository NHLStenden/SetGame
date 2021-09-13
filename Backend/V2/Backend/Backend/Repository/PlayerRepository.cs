using Backend.Models;
using Dapper.Logging;

namespace Backend.Repository
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}