using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SetContext _db;

        public UnitOfWork(SetContext db)
        {
            _db = db;
        }
        
        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}