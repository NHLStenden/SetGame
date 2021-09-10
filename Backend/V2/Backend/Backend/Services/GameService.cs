using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;

namespace Backend.Services
{
    public class GameService : IGameService
    {
        private readonly SetContext _db;

        public GameService(SetContext db)
        {
            _db = db;
        }
        
        // public Game StartNewGame(int playerId)
        // {
        //     var deck = new DeckService().CreateDeck();
        //     var player = new PlayerService().GetById(playerId);
        //     
        //     _db.Games.Add(new Game()
        //     {
        //         GameId = _db.Games.Count + 1,
        //         Deck = deck,
        //         DeckId = deck.DeckId,
        //         Player = player,
        //         PlayerId = player.PlayerId
        //     });
        //
        //     return _db.Games[^1];
        // }

        public Card[] DrawCardsFromDeck(int gameId, int numCards)
        {
            Game game = _db.Games.First(x => x.GameId == gameId);
            //Todo throw deck not found exception or not found expcetion
            

            int endIndex = Math.Min(81, game.CardIndex + numCards);
            var deckCards = game.Deck.Cards[game.CardIndex..endIndex];
            game.CardIndex = endIndex;
            
            return deckCards;
        }
        
        public int StartNewGame(int playerId, int? deckId)
        {
            DeckService deckService = new DeckService(_db);
            Game game = new Game()
            {
                GameId = _db.Games.Max(x => x.GameId) + 1,
                Deck = deckId.HasValue ? 
                    deckService.GetById(deckId) : deckService.CreateDeck(), 
            };
            game.DeckId = game.Deck.DeckId;
            game.PlayerId = playerId;
                
            _db.Games.Add(game);

            return game.GameId;
        }
    }
}