using System.Threading.Tasks;

namespace Backend.Repository
{
    public interface IUnitOfWork
    {
        public Task SaveChangesAsync();
    }
}