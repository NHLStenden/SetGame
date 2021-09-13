using Backend.Models;
using Microsoft.Extensions.Configuration;

namespace Backend.Repository
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}