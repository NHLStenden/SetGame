using Backend.Models;
using Microsoft.Extensions.Configuration;

namespace Backend.Repository
{
    public class CardRepository : GenericRepository<Card>, ICardRepository
    {
        public CardRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}