using Backend.Models;
using Dapper.Logging;

namespace Backend.Repository
{
    public class DeckRepository : GenericRepository<Deck>, IDeckRepository
    {
        public DeckRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}