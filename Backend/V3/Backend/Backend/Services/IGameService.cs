using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IGameService
    {
        Task<Game> StartNewGame(int playerId);
        Task<IList<Card>> DrawCardsFromDeck(int gameId, int numberOfCards);
        Task<SetResult> CheckSet(int gameId, int[] cardIds);
        Task<IList<Card>> GetCardsOnTable(int gameId);
        Task<IList<Card>> GetCardsOnTable(int gameId, int[] cardIds);
        Task<IList<IList<Card>>> GetAllSetsOnTable(int gameId);
        Task<bool> SubmitSet(int gameId, int[] cardIds);
        Task<int> CalculateComplexityForCardsOnTable(int game);
    }
}