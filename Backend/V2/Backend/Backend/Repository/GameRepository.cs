using Backend.Models;
using Dapper.Logging;
using Microsoft.Extensions.Configuration;

namespace Backend.Repository
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}