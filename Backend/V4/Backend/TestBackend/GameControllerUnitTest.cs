using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Controllers;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;
using YamlDotNet.Serialization.NodeTypeResolvers;

namespace TestBackend
{
    public class GameControllerUnitTest
    {
        [Fact]
        public async Task CheckSet_InvalidNumberOfCards_ArgumentException()
        {
            var game = new Game()
            {
                CardIndex = -1, CardsOnTable = new List<CardOnTable>()
                {
                    new CardOnTable()
                    {
                        Card = new Card()
                            { Color = Color.Green, Fill = Fill.Solid, Shape = Shape.Pill, Id = 1, NrOfShapes = 3 }
                    }
                }
            };

            var gameDto = new GameDto()
            {
                CardsOnTable = game.CardsOnTable.Select(x => x.Card).ToList(),
                Complexity = 10, PlayerId = 1, PlayerName = "Joris"
            };
            
            
            var uof = Substitute.For<IUnitOfWork>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameService = Substitute.For<IGameService>();
            gameService.StartNewGame(1).Returns(game);
            
            
            var mapper = Substitute.For<IMapper>();
            mapper.Map<GameDto>(game).Returns(gameDto);

            //arrange
            var sut = new GameController(uof, gameRepository, gameService, mapper);

            //act
            var resultGameDto = await sut.StartNewGame(1);

            //asserts
            gameService.Received().StartNewGame(1);
            uof.Received().SaveChangesAsync();
            mapper.Received().Map<GameDto>(game);
            resultGameDto.Should().BeEquivalentTo(gameDto);
        }
        
        
    }
}