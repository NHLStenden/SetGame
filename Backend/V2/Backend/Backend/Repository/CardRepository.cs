using Backend.Models;
using Dapper.Logging;
using Microsoft.Extensions.Configuration;

namespace Backend.Repository
{
    public class CardRepository : GenericRepository<Card>, ICardRepository
    {
        public CardRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
    }
}