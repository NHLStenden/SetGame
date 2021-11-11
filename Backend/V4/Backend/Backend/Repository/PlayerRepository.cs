using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(SetContext db) : base(db)
        {
        }


    }
}