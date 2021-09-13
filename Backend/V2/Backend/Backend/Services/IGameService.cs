using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IGameService
    {
        Task<Card[]> DrawCardsFromDeck(int gameId, int numCards);
        Task<int> StartNewGame(int playerId, int? deckId = null);
    }
}