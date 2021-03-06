using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface IDeckService
    {
        Task<Deck> CreateDeck();
        Task<List<Card>> CreateCards();
    }
}