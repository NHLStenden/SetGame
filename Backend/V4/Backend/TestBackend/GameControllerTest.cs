using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using FluentAssertions;
using Xunit;

namespace TestBackend
{
    //Todo: add concurrency test (DrawCards) in GameService.cs
    //Todo: add concurrency test
    //Todo: add test that uses different games (gameIds)
    
    [Collection("IntegrationTests")]
    public class GameControllerTests : IntegrationTest
    {
        [Fact]
        public async Task DrawCardFrom_Deck_InvalidNumberOfCards()
        {
            int gameId = 1;
            var error = await PostRequestAsync<Error>($"/Game/DrawCards/{gameId}",
                parameters: new {numberOfCards = -1}, ensureStatusCodes: false);

            error.StatusCode.Should().Be(500);
            error.Message.Should().Be("Internal Server Error");
        }
        
        [Fact]
        public async Task DrawCardsFrom_Deck_ThreeCards()
        {
            int gameId = 1;
            int numberOfCards = 3;
            var cards = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards});

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
            var error = await PostRequestAsync<Error>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards = 41}, ensureStatusCodes: false);
            
            error.StatusCode.Should().Be(500);
            error.Message.Should().Be("Internal Server Error");
        }
        
        [Fact]
        public async Task DrawCards_DrawCardsUntilDeckIsEmpty_EmptyList()
        {
            int gameId = 1;
            var cards = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards = 41});
            cards.Should().HaveCount(41);
            cards.Should().OnlyHaveUniqueItems();

            var nextHand = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards = 38});
            nextHand.Should().HaveCount(38);
            nextHand.Should().OnlyHaveUniqueItems();
            
            var allCards = cards.Concat(nextHand).ToList();
            allCards.Should().HaveCount(41+38);
            allCards.Should().OnlyHaveUniqueItems();

            nextHand = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards = 3});
            nextHand.Should().HaveCount(2);

            allCards = allCards.Concat(nextHand).ToList();
            allCards.Should().HaveCount(81);
            allCards.Should().OnlyHaveUniqueItems();
            
            nextHand = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards = 3});
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
            
            var cards = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards = 3});
            var cardsOnTable = await GetRequestAsync<Card[]>($"/Game/GetCardsOnTable/{gameId}");

            cards.Should().HaveCount(3);
            cardsOnTable.Should().HaveCount(3);

            cardsOnTable.Should().BeEquivalentTo(cards);
        }

        [Fact]
        public async Task StartNewGame_ValidPlayerId_GameObject()
        {
            int playerId = 1;

            var game = await PostRequestAsync<GameDto>($"/Game/StartNewGame/{playerId}");

            game.Should().NotBeNull();

            game.Id.Should().Be(2);
            //game.Deck.Cards.Should().HaveCount(81);
            game.Complexity = 1;
            game.CardsOnTable.Should().HaveCount(0);
            game.PlayerId.Should().Be(1);
            game.PlayerName.Should().Be("Joris");
        }

        [Fact]
        public async Task GetByIdAsync_ValidGameId_GameObject()
        {
            int gameId = 1;
            
            var game = await GetRequestAsync<GameDto>($"/Game/{gameId}");
            
            game.PlayerName.Should().Be("Joris");
            game.CardsOnTable.Should().HaveCount(0);
            game.Complexity.Should().Be(-1);
        }

        [Fact]
        public async Task VerifyComplexityIsUpdatedAfterDrawCards_ValidGameIdDraw12Cards_ComplexityOfOne()
        {
            int gameId = 1;
            
            var game = await GetRequestAsync<GameDto>($"/Game/{gameId}");
            game.Complexity.Should().Be(-1);

            int complexity = await GetRequestAsync<int>($"/Game/CalculateComplexityForCardsOnTable/{gameId}");
            complexity.Should().Be(-1);
            
            int numberOfCards = 12;
            var _ = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards});
            
            game = await GetRequestAsync<GameDto>($"/Game/{gameId}");
            game.Complexity.Should().Be(1);
            
            complexity = await GetRequestAsync<int>($"/Game/CalculateComplexityForCardsOnTable/{gameId}");
            complexity.Should().Be(1);            
        }
        
        [Fact]
        public async Task CalculateComplexityForCardsOnTable_ValidGameIdDraw12Cards_ComplexityOfOne()
        {
            int gameId = 1;
            
            var game = await GetRequestAsync<GameDto>($"/Game/{gameId}");
            game.Complexity.Should().Be(-1);

            int complexity = await GetRequestAsync<int>($"/Game/CalculateComplexityForCardsOnTable/{gameId}");
            complexity.Should().Be(-1);
            
            int numberOfCards = 12;
            var _ = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards});
            
            game = await GetRequestAsync<GameDto>($"/Game/{gameId}");
            game.Complexity.Should().Be(1);
            
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
            var _ = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards}
            );
            var setResult = await GetRequestAsync<SetResult>($"/Game/CheckSet/{gameId}", 
                new {  cardIds = new List<int>() {49, 51, 57} }
            );
            
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
            var _ = await PostRequestAsync<Card[]>(
                $"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards}
            );
            var cardIds = new List<int>() {49, 51, 57};
            var success = await PostRequestAsync<bool>(
                $"/Game/SubmitSet/{gameId}", 
                parameters: new { cardIds }
            );

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
            var _ = await PostRequestAsync<Card[]>(
                $"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards}
            );
            var cardIds = new List<int>() {48, 53, 56};
            var success = await PostRequestAsync<bool>(
                $"/Game/SubmitSet/{gameId}", 
                parameters: new { cardIds }
            );

            success.Should().BeFalse();
            
            var cardsOnTable = await GetRequestAsync<Card[]>($"/Game/GetCardsOnTable/{gameId}");
            cardsOnTable.Should().HaveCount(12);
        }

        [Fact]
        public async Task CreateAndDeleteGame_CorrectGameId_CreatedAndDeleteGameWithDependecies()
        {
            int numberOfCards = 12;
            int playerId = 1;

            var game = await PostRequestAsync<GameDto>($"/Game/StartNewGame/{playerId}");
            
            var _ = await PostRequestAsync<Card[]>($"/Game/DrawCards/{game.Id}", 
                parameters: new {numberOfCards});

            var response = await Client.DeleteAsync($"/Game/{game.Id}");
            response.EnsureSuccessStatusCode();
        }
        
        [Fact]
        public async Task FindAllSets_CorrectGameId_OneSetFound()
        {
            int gameId = 1;
            int numberOfCards = 12;
            var _ = await PostRequestAsync<Card[]>($"/Game/DrawCards/{gameId}", 
                parameters: new {numberOfCards});
            var cardIds = new List<int>() {49, 51, 57};
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