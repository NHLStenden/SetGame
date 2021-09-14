using Backend.Models;

namespace Backend.Services
{
    public interface IDeckService
    {
        Deck CreateDeck();
        Deck GetById(int? deckId);
    }
}