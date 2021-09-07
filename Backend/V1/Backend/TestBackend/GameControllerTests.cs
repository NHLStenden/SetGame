using System;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using FluentAssertions;
using NUnit.Framework;

namespace TestBackend
{
    public class GameControllerTests : IntegrationTest
    {
        [Test]
        public async Task GetCardsFrom_Deck_ThreeCards()
        {
            var cards = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 3});

            cards.Should().HaveCount(3);
            cards.Should().OnlyHaveUniqueItems();
            
            var expectedCards = new Card[]
            {
                new Card() {
                    CardId = 1,
                    Color = Backend.Models.Color.Violet,
                    Fill = Backend.Models.Fill.Striped,
                    NrOfShapes = 3,
                    Shape = Shape.Diamond
                }, new Card()
                {
                    CardId = 2,
                    Color = Backend.Models.Color.Violet,
                    Fill = Backend.Models.Fill.Striped,
                    NrOfShapes = 1,
                    Shape = Shape.Diamond
                }, new Card()
                {
                    CardId = 3,
                    Color = Color.Red,
                    Fill = Backend.Models.Fill.Hollow,
                    NrOfShapes = 3,
                    Shape = Shape.Wave                    
                }
            };

            cards.Should().BeEquivalentTo(expectedCards);
        }

        [Test]
        public async Task DrawCards_InvalidDeckId_Exception()
        {

            Func<Task> request = async () => await GetRequest<Card[]>("/Game/DrawCards", new {gameId = -1, numberOfCards = 41});
            request.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Test]
        public async Task DrawCards_DeckIsEmpty_EmptyList()
        {
            var cards = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 41});
            cards.Should().HaveCount(41);
            cards.Should().OnlyHaveUniqueItems();

            var nextHand = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 38});
            nextHand.Should().HaveCount(38);
            nextHand.Should().OnlyHaveUniqueItems();
            
            var allCards = cards.Concat(nextHand);
            allCards.Should().HaveCount(41+38);
            allCards.Should().OnlyHaveUniqueItems();

            nextHand = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 3});
            nextHand.Should().HaveCount(2);

            allCards = allCards.Concat(nextHand);
            allCards.Should().HaveCount(81);
            allCards.Should().OnlyHaveUniqueItems();
            
            nextHand = await GetRequest<Card[]>("/Game/DrawCards", new {gameId = 1, numberOfCards = 3});
            nextHand.Should().HaveCount(0);
        }
    }
}