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
            Func<Task> request = async () => await GetRequest<Card[]>("/Game/DrawCards",
                new {gameId = 1, numberOfCards = -1});

            await request.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
        
        [Fact]
        public async Task GetCardsFrom_Deck_ThreeCards()
        {
            var cards = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 3});

            cards.Should().HaveCount(3);
            cards.Should().OnlyHaveUniqueItems();
            
            var expectedCards = new[]
            {
                new Card
                {
                    CardId = 1,
                    Color = Color.Violet,
                    Fill = Fill.Striped,
                    NrOfShapes = 3,
                    Shape = Shape.Diamond
                }, new Card
                {
                    CardId = 2,
                    Color = Color.Violet,
                    Fill = Fill.Striped,
                    NrOfShapes = 1,
                    Shape = Shape.Diamond
                }, new Card
                {
                    CardId = 3,
                    Color = Color.Red,
                    Fill = Fill.Hollow,
                    NrOfShapes = 3,
                    Shape = Shape.Wave                    
                }
            };

            cards.Should().BeEquivalentTo(expectedCards);
        }

        [Fact]
        public async Task DrawCards_InvalidDeckId_Exception()
        {
            Func<Task> request = async () => await GetRequest<Card[]>("/Game/DrawCards", new {gameId = -1, numberOfCards = 41});
            await request.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task DrawCards_DeckIsEmpty_EmptyList()
        {
            var cards = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 41});
            cards.Should().HaveCount(41);
            cards.Should().OnlyHaveUniqueItems();

            var nextHand = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 38});
            nextHand.Should().HaveCount(38);
            nextHand.Should().OnlyHaveUniqueItems();
            
            var allCards = cards.Concat(nextHand).ToList();
            allCards.Should().HaveCount(41+38);
            allCards.Should().OnlyHaveUniqueItems();

            nextHand = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 3});
            nextHand.Should().HaveCount(2);

            allCards = allCards.Concat(nextHand).ToList();
            allCards.Should().HaveCount(81);
            allCards.Should().OnlyHaveUniqueItems();
            
            nextHand = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 3});
            nextHand.Should().HaveCount(0);
        }
    }
}