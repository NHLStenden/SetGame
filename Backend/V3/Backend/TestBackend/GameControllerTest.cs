using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
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
                new {numberOfCards = 3});

            cards.Should().HaveCount(3);
            cards.Should().OnlyHaveUniqueItems();
            
            var expectedCards = new[]
            {
                new Card
                {
                    CardIndex = 39,
                    Color = Color.Violet,
                    Fill = Fill.Solid,
                    Id = 1,
                    NrOfShapes = 1,
                    Shape = Shape.Wave
                }, new Card
                {
                    CardIndex = 57,
                    Color = Color.Violet,
                    Fill = Fill.Hollow,
                    Id = 2,
                    NrOfShapes = 3,
                    Shape = Shape.Pill,
                }, new Card
                {
                    CardIndex = 56,
                    Color = Color.Green,
                    Fill = Fill.Solid,
                    Id = 3,
                    NrOfShapes = 2,
                    Shape = Shape.Wave,                  
                }
            };

            cards.Should().BeEquivalentTo(expectedCards);
        }

        [Fact]
        public async Task DrawCards_InvalidDeckId_Exception()
        {
            int gameId = 1;
            Func<Task> request = async () => await GetRequest<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 41});
            await request.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task DrawCards_UntilDeckIsEmpty_EmptyList()
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
    }
}