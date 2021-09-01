using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;

namespace Backend.Services
{
    public class GameService
    {
        private static List<Game> Games = new List<Game>();

        public Game StartNewGame(int playerId)
        {
            var deck = new DeckService().CreateDeck();
            var player = new PlayerService().GetById(playerId);
            
            Games.Add(new Game()
            {
                GameId = Games.Count + 1,
                Deck = deck,
                DeckId = deck.DeckId,
                Player = player,
                PlayerId = player.PlayerId
            });

            return Games[^1];
        }

        public Card[] DrawCardsFromDeck(int gameId, int numCards)
        {
            Game game = Games.First(x => x.GameId == gameId);

            int endIndex = Math.Min(81, game.CardIndex + numCards);
            var deckCards = game.Deck.Cards[game.CardIndex..endIndex];
            game.CardIndex = endIndex;
            
            return deckCards;
        }
    }
}