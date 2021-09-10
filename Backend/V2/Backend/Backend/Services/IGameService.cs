using Backend.Models;

namespace Backend.Services
{
    public interface IGameService
    {
        Card[] DrawCardsFromDeck(int gameId, int numCards);
        int StartNewGame(int playerId, int? deckId = null);
    }
}