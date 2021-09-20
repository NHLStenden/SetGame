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
    }
}