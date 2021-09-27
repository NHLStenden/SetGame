using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend;
using Backend.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace TestBackend
{
    public class CrudControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetByIdAsync_CorrectPlayerId_Player()
        {
            var playerId = 1;
            var player = await GetRequestAsync<Player>($"/Player/{playerId}");

            player.Id.Should().Be(1);
            player.Name.Should().Be("Joris");
        }
        
        [Fact]
        public async Task GetAsync_CorrectPlayerId_Player()
        {
            var players = await GetRequestAsync<Player[]>($"/Player");

            players.Should().HaveCount(1);
            players[0].Id.Should().Be(1);
            players[0].Name.Should().Be("Joris");
        }
        
        [Fact]
        public async Task CreateAndDeleteGame_CorrectGameId_CreatedAndDeleteGame()
        {
            string name = "New Player";
            var player = await PostRequestAsync<Player>($"/Player/", new Player
            {
                Name = name
            });

            player.Id.Should().Be(2);
            player.Name.Should().Be(name);
        }
        
        [Fact]
        public async Task CreateAndDeleteGame_IncorrectPlayer_BadRequest()
        {
            string name = "New Player";
            var error = await PostRequestAsync<Error>($"/Player/", new Player
            {
                Id = 1,
                Name = name
            }, ensureStatusCodes: false);

            error.StatusCode.Should().Be(500);
            error.Message.Should().Be("Internal Server Error");
        }
        
        [Fact]
        public async Task UpdatePlayer_CorrectPlayerNameAndId_UpdatedPlayerName()
        {
            string name = "Updated Player Name";
            int playerId = 1;
            var player = await PutRequestAsync<Player>($"/Player/{playerId}", new Player
            {
                Id = playerId,
                Name = name
            });

            player.Id.Should().Be(1);
            player.Name.Should().Be(name);
        }
        
        [Fact]
        public async Task UpdatePlayer_IncorrectPlayer_BadRequest()
        {
            string name = "New Player";
            var error = await PutRequestAsync<BadRequestResult>($"/Player/1", new Player
            {
                Id = 2,
                Name = name
            }, ensureStatusCodes: false);

            error.StatusCode.Should().Be(400);
        } 
    }
}