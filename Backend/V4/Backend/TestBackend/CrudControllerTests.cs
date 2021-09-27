using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
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
        public async Task GetAsync_None_Players()
        {
            var players = await GetRequestAsync<Player[]>($"/Player");

            players.Should().HaveCount(1);
            players[0].Id.Should().Be(1);
            players[0].Name.Should().Be("Joris");
        }
        
        [Fact]
        public async Task Create_Player_CreatePlayer()
        {
            string name = "New Player";
            var player = await PostRequestAsync<Player>($"/Player/", new Player
            {
                Name = name
            });

            player.Id.Should().Be(2);
            player.Name.Should().Be(name);
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputAndProblemDetails))]
        public async Task CreatePlayer_InvalidInput_BadRequest(Player player, KeyValuePair<string, string> validator)
        {
            var response = await Client.PostAsJsonAsync("/player", player);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            problemDetails.Errors.Should().HaveCount(1);
            problemDetails.Errors.Should().ContainKey(validator.Key);
            problemDetails.Errors[validator.Key][0].Should().Be(validator.Value);
        }

        public static IEnumerable<object[]> GetInvalidInputAndProblemDetails()
        {
            var testData = new List<object[]>()
            {
                new object[]
                {
                    GetValidPlayer().CloneWith(x => x.Name = new string('c',51)),
                    new KeyValuePair<string, string>("Name", "The field Name must be a string or array type with a maximum length of '50'.")
                },
                new object[]
                {
                    GetValidPlayer().CloneWith(x => x.Name = "J"),
                    new KeyValuePair<string,string>("Name", "The field Name must be a string or array type with a minimum length of '2'.")
                },
                new object[]
                {
                    GetValidPlayer().CloneWith(x => x.Name = null),
                    new KeyValuePair<string, string>("Name", "The Name field is required.")
                }
            };
            return testData;
        }

        private static Player GetValidPlayer()
        {
            return new Player()
            {
                Name = "Joris"
            };
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