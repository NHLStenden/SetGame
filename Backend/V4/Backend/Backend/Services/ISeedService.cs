using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services
{
    public interface ISeedService
    {
        string GetDataPath();
        void Seed(SetContext db, IGameService gameService, IPlayerRepository playerRepository);
    }
}