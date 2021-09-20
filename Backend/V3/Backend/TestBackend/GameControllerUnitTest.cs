using System;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Repository;
using Backend.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace TestBackend
{
    public class GameControllerUnitTest
    {
        [Fact]
        public async Task CheckSet_InvalidNumberOfCards_ArgumentException()
        {
            var deckService = new Mock<IDeckService>();
            var gameRepository = new Mock<IGameRepository>();
            var playerRepository = new Mock<IPlayerRepository>();
            var sut = new GameService(deckService.Object, gameRepository.Object, playerRepository.Object);
            
            Func<Task> checkSet = async () => await sut.CheckSet(1, new []{1,2});

            await checkSet.Should().ThrowAsync<ArgumentException>();
        }
        
        [Fact]
        public async Task CheckSet_ValidSet_CorrectSet()
        {
            var deckService = new Mock<IDeckService>();
            var gameRepository = new Mock<IGameRepository>();
            var playerRepository = new Mock<IPlayerRepository>();
            var sut = new GameService(deckService.Object, gameRepository.Object, playerRepository.Object);

            

            // Func<Task> request = async () => await GetRequest<Card[]>($"/Game/DrawCards/{gameId}",
            //     new {numberOfCards = -1});
            //
            // await request.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }
        
        
    }
}