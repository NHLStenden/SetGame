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
            
            var expectedCards = new Card[]
            {
                new Card
                {
                    Id = 58,
                    Shape = Shape.Wave,
                    Fill = Fill.Hollow,
                    Color = Color.Red,
                    NrOfShapes = 2
                },
                new Card
                {
                    Id = 57,
                    Shape = Shape.Pill,
                    Fill = Fill.Striped,
                    Color = Color.Red,
                    NrOfShapes = 3
                },
                new Card
                {
                    Id = 56,
                    Shape = Shape.Diamond,
                    Fill = Fill.Hollow,
                    Color = Color.Red,
                    NrOfShapes = 3
                }
            };

            cards.Should().BeEquivalentTo(expectedCards);
        }

        [Fact]
        public async Task DrawCards_InvalidDeckId_Exception()
        {
            int gameId = -1;
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