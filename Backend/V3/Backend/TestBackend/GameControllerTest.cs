using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Services;
using FluentAssertions;
using Xunit;

namespace TestBackend
{
    public class GameControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetCardFrom_Deck_InvalidNumberOfCards()
        {
            int gameId = 1;
            Func<Task> request = async () => await GetRequest<Card[]>($"/Game/DrawCards/{gameId}",
                new {numberOfCards = -1});

            await request.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
        
        [Fact]
        public async Task GetCardsFrom_Deck_ThreeCards()
        {
            int gameId = 1;
            int numberOfCards = 3;
            var cards = await GetRequest<Card[]>($"/Game/DrawCards/{gameId}", 
                new {numberOfCards = numberOfCards});

            cards.Should().HaveCount(numberOfCards);
            cards.Should().OnlyHaveUniqueItems();

            var expectedCards = new TestSeedService().GetGame().Deck.Cards.Select(x => x.Card).Take(numberOfCards);

            cards.Should().BeEquivalentTo(expectedCards, options => 
                options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task DrawCards_InvalidDeckId_Exception()
        {
            int gameId = -1;
            Func<Task> request = async () => await GetRequest<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 41});
            await request.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task DrawCards_DrawCardsUntilDeckIsEmpty_EmptyList()
        {
            int gameId = 1;
            var cards = await GetRequest<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 41});
            cards.Should().HaveCount(41);
            cards.Should().OnlyHaveUniqueItems();

            var nextHand = await GetRequest<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 38});
            nextHand.Should().HaveCount(38);
            nextHand.Should().OnlyHaveUniqueItems();
            
            var allCards = cards.Concat(nextHand).ToList();
            allCards.Should().HaveCount(41+38);
            allCards.Should().OnlyHaveUniqueItems();

            nextHand = await GetRequest<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 3});
            nextHand.Should().HaveCount(2);

            allCards = allCards.Concat(nextHand).ToList();
            allCards.Should().HaveCount(81);
            allCards.Should().OnlyHaveUniqueItems();
            
            nextHand = await GetRequest<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 3});
            nextHand.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetCardsOnTable_ValidGameWithoutCardsOnTable_NoCardsOnTable()
        {
            int gameId = 1;
            var cardsOnTable = await GetRequest<Card[]>($"/Game/GetCardsOnTable/{gameId}", new {numberOfCards = 3});

            cardsOnTable.Should().HaveCount(0);
        }
        
        [Fact]
        public async Task GetCardsOnTable_DrawCardsThenGetCardsOnTable_CurrentCardsOnTable()
        {
            int gameId = 1;
            
            var cards = await GetRequest<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 3});
            var cardsOnTable = await GetRequest<Card[]>($"/Game/GetCardsOnTable/{gameId}", new {numberOfCards = 3});

            cards.Should().HaveCount(3);
            cardsOnTable.Should().HaveCount(3);

            cardsOnTable.Should().BeEquivalentTo(cards);
        }

        [Fact]
        public async Task StartNewGame_ValidPlayerId_GameObject()
        {
            int playerId = 1;

            var game = await GetRequest<Game>($"/Game/StartNewGame/{playerId}");

            game.Should().NotBeNull();

            game.Id.Should().Be(2);
            game.Deck.Cards.Should().HaveCount(81);
            game.Deck.Complexity = 1;
            game.CardsOnTable.Should().HaveCount(0);
            game.PlayerId.Should().Be(1);
            game.Player.Name.Should().Be("Joris");
        }
        
        
    }
}