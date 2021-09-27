using Backend.Models;

namespace Backend.Repository
{
    public class CardRepository : GenericRepository<Card>, ICardRepository
    {
        public CardRepository(SetContext db) : base(db)
        {
        }
    }
}