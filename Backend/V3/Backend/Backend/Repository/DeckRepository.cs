using Backend.Models;

namespace Backend.Repository
{
    public class DeckRepository : GenericRepository<Deck>, IDeckRepository
    {
        public DeckRepository(SetContext db) : base(db)
        {
        }
    }
}