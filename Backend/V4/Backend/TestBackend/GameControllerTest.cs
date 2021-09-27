using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend;
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
            var error = await GetRequestAsync<Error>($"/Game/DrawCards/{gameId}",
                new {numberOfCards = -1});

            error.StatusCode.Should().Be(500);
            error.Message.Should().Be("Internal Server Error");
        }
        
        [Fact]
        public async Task GetCardsFrom_Deck_ThreeCards()
        {
            int gameId = 1;
            int numberOfCards = 3;
            var cards = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                new {numberOfCards});

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
            var error = await GetRequestAsync<Error>($"/Game/DrawCards/{gameId}", new {numberOfCards = 41});
            
            error.StatusCode.Should().Be(500);
            error.Message.Should().Be("Internal Server Error");
        }
        
        [Fact]
        public async Task DrawCards_DrawCardsUntilDeckIsEmpty_EmptyList()
        {
            int gameId = 1;
            var cards = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 41});
            cards.Should().HaveCount(41);
            cards.Should().OnlyHaveUniqueItems();

            var nextHand = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 38});
            nextHand.Should().HaveCount(38);
            nextHand.Should().OnlyHaveUniqueItems();
            
            var allCards = cards.Concat(nextHand).ToList();
            allCards.Should().HaveCount(41+38);
            allCards.Should().OnlyHaveUniqueItems();

            nextHand = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 3});
            nextHand.Should().HaveCount(2);

            allCards = allCards.Concat(nextHand).ToList();
            allCards.Should().HaveCount(81);
            allCards.Should().OnlyHaveUniqueItems();
            
            nextHand = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 3});
            nextHand.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetCardsOnTable_ValidGameWithoutCardsOnTable_NoCardsOnTable()
        {
            int gameId = 1;
            var cardsOnTable = await GetRequestAsync<Card[]>($"/Game/GetCardsOnTable/{gameId}");

            cardsOnTable.Should().HaveCount(0);
        }
        
        [Fact]
        public async Task GetCardsOnTable_DrawCardsThenGetCardsOnTable_CurrentCardsOnTable()
        {
            int gameId = 1;
            
            var cards = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", new {numberOfCards = 3});
            var cardsOnTable = await GetRequestAsync<Card[]>($"/Game/GetCardsOnTable/{gameId}");

            cards.Should().HaveCount(3);
            cardsOnTable.Should().HaveCount(3);

            cardsOnTable.Should().BeEquivalentTo(cards);
        }

        [Fact]
        public async Task StartNewGame_ValidPlayerId_GameObject()
        {
            int playerId = 1;

            var game = await GetRequestAsync<Game>($"/Game/StartNewGame/{playerId}");

            game.Should().NotBeNull();

            game.Id.Should().Be(2);
            game.Deck.Cards.Should().HaveCount(81);
            game.Deck.Complexity = 1;
            game.CardsOnTable.Should().HaveCount(0);
            game.PlayerId.Should().Be(1);
            game.Player.Name.Should().Be("Joris");
        }

        [Fact]
        public async Task GetByIdAsync_ValidGameId_GameObject()
        {
            int gameId = 1;
            
            var game = await GetRequestAsync<Game>($"/Game/{gameId}");

            game.Deck.Cards.Should().HaveCount(81);
            game.Deck.Cards.Should().BeInAscendingOrder(x => x.Order);
            game.Player.Name.Should().Be("Joris");
            game.CardIndex.Should().Be(0);
            game.CardsOnTable.Should().HaveCount(0);
            game.Deck.Complexity.Should().Be(-1);
        }

        [Fact]
        public async Task VerifyComplexityIsUpdatedAfterDrawCards_ValidGameIdDraw12Cards_ComplexityOfOne()
        {
            int gameId = 1;
            
            var game = await GetRequestAsync<Game>($"/Game/{gameId}");
            game.Deck.Complexity.Should().Be(-1);

            int complexity = await GetRequestAsync<int>($"/Game/CalculateComplexityForCardsOnTable/{gameId}");
            complexity.Should().Be(-1);
            
            int numberOfCards = 12;
            var _ = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                new {numberOfCards});
            
            game = await GetRequestAsync<Game>($"/Game/{gameId}");
            game.Deck.Complexity.Should().Be(1);
            
            complexity = await GetRequestAsync<int>($"/Game/CalculateComplexityForCardsOnTable/{gameId}");
            complexity.Should().Be(1);            
        }
        
        [Fact]
        public async Task CalculateComplexityForCardsOnTable_ValidGameIdDraw12Cards_ComplexityOfOne()
        {
            int gameId = 1;
            
            var game = await GetRequestAsync<Game>($"/Game/{gameId}");
            game.Deck.Complexity.Should().Be(-1);

            int complexity = await GetRequestAsync<int>($"/Game/CalculateComplexityForCardsOnTable/{gameId}");
            complexity.Should().Be(-1);
            
            int numberOfCards = 12;
            var _ = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                new {numberOfCards});
            
            game = await GetRequestAsync<Game>($"/Game/{gameId}");
            game.Deck.Complexity.Should().Be(1);
            
            complexity = await GetRequestAsync<int>($"/Game/CalculateComplexityForCardsOnTable/{gameId}");
            complexity.Should().Be(1);            
        }

        [Fact]
        public async Task CheckSet_IncorrectSetSize_ArgumentException()
        {
            int gameId = 1;
            var error = await GetRequestAsync<Error>($"/Game/CheckSet/{gameId}",
                new {cardIds = new List<int>() { 58, 57 }}, ensureStatusCode: false);

            error.StatusCode.Should().Be(500);
            error.Message.Should().Be("Internal Server Error");

            error = await GetRequestAsync<Error>($"/Game/CheckSet/{gameId}",
                new {}, ensureStatusCode: false);

            error.StatusCode.Should().Be(500);
            error.Message.Should().Be("Internal Server Error");
        } 
        
        [Fact]
        public async Task CheckSet_CorrectSet_CorrectSetResult()
        {
            int gameId = 1;
            int numberOfCards = 12;
            var _ = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                new {numberOfCards});
            var setResult = await GetRequestAsync<SetResult>($"/Game/CheckSet/{gameId}", 
                new
            {
                cardIds = new List<int>() {48, 51, 56}
            });
            
            var expected = new SetResult
            {
                CorrectSet = true,
                ColorsCorrect = true,
                ShapeCorrect = true,
                FillCorrect = true,
                NrOfShapeCorrect = true,
                ColorSame = false,
                ColorDifferent = true,
                ShapeSame = true,
                ShapeDifferent = false,
                FillSame = false,
                FillDifferent = true,
                NrOfShapeSame = false,
                NrOfShapeDifferent = true
            };

            setResult.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task SubmitSet_CorrectSet_SuccessIsTrueAndRemoveSetFromCardsOnTable()
        {
            int gameId = 1;
            int numberOfCards = 12;
            var _ = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                new {numberOfCards});
            var cardIds = new List<int>() {48, 51, 56};
            var success = await GetRequestAsync<bool>($"/Game/SubmitSet/{gameId}", 
                new
                {
                    cardIds
                });

            success.Should().BeTrue();
            
            var cardsOnTable = await GetRequestAsync<Card[]>($"/Game/GetCardsOnTable/{gameId}");
            cardsOnTable.Should().HaveCount(9);

            cardsOnTable.Should().NotContain(x => cardIds.Contains(x.Id));
        }
        
        [Fact]
        public async Task SubmitSet_IncorrectSet_SuccessIsFalseAndCardsOnTableIsUnchanged()
        {
            int gameId = 1;
            int numberOfCards = 12;
            var _ = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                new {numberOfCards});
            var cardIds = new List<int>() {48, 53, 56};
            var success = await GetRequestAsync<bool>($"/Game/SubmitSet/{gameId}", 
                new
                {
                    cardIds
                });

            success.Should().BeFalse();
            
            var cardsOnTable = await GetRequestAsync<Card[]>($"/Game/GetCardsOnTable/{gameId}");
            cardsOnTable.Should().HaveCount(12);
        }

        [Fact]
        public async Task CreateAndDeleteGame_CorrectGameId_CreatedAndDeleteGameWithDependecies()
        {
            int numberOfCards = 12;
            int playerId = 1;

            var game = await GetRequestAsync<Game>($"/Game/StartNewGame/{playerId}");
            
            var _ = await GetRequestAsync<Card[]>($"/Game/DrawCards/{game.Id}", 
                new {numberOfCards});

            var response = await Client.DeleteAsync($"/Game/{game.Id}");
            response.EnsureSuccessStatusCode();
        }
        
        [Fact]
        public async Task FindAllSets_CorrectGameId_OneSetFound()
        {
            int gameId = 1;
            int numberOfCards = 12;
            var _ = await GetRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                new {numberOfCards});
            var cardIds = new List<int>() {48, 53, 56};
            var success = await GetRequestAsync<List<List<Card>>>($"/Game/GetAllSetsOnTable/{gameId}", 
                new
                {
                    cardIds
                });

            success.Should().HaveCount(1);
            success[0].Should().HaveCount(3);

            success[0].Should().Contain(x => cardIds.Contains(x.Id));
            //success.Should().BeFalse();
            
            var cardsOnTable = await GetRequestAsync<Card[]>($"/Game/GetCardsOnTable/{gameId}");
            cardsOnTable.Should().HaveCount(12);
        }
        
    }
}