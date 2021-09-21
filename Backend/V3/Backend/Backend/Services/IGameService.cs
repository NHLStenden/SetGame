using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IGameService
    {
        Task<Game> StartNewGame(int playerId);
        Task<List<Card>> DrawCardsFromDeck(int gameId, int numberOfCards);

        Task<SetResult> CheckSet(int gameId, int[] cardIds);
        
        Task<List<Card>> GetCardsOnTable(int gameId);
        Task<List<Card>> GetCardsOnTable(int gameId, int[] cardIds);
        Task<List<IList<Card>>> FindAllSetsOnTable(int gameId);
        Task<bool> SubmitSet(int gameId, int[] cardIds);
    }
}