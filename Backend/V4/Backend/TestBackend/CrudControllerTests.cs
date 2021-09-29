using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend;
using Backend.Controllers;
using Backend.Models;
using Backend.ViewModels;
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
            var player = await PostRequestAsync<Player>($"/Player", new PlayerCreateModel()
            {
                Name = name, Email = "test@test.com", EmailValidate = "test@test.com"
            });

            player.Id.Should().Be(2);
            player.Name.Should().Be(name);
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputAndProblemDetails))]
        public async Task CreatePlayer_InvalidInput_BadRequest(PlayerCreateModel playerCreateModel, KeyValuePair<string, string> validator)
        {
            var response = await Client.PostAsJsonAsync("/Player", playerCreateModel);

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
                    GetValidPlayerInputModel().CloneWith(x => x.Name = new string('c',51)),
                    new KeyValuePair<string, string>("Name", "The field Name must be a string or array type with a maximum length of '50'.")
                },
                new object[]
                {
                    GetValidPlayerInputModel().CloneWith(x => x.Name = "J"),
                    new KeyValuePair<string,string>("Name", "The field Name must be a string or array type with a minimum length of '2'.")
                },
                new object[]
                {
                    GetValidPlayerInputModel().CloneWith(x => x.Name = null),
                    new KeyValuePair<string, string>("Name", "The Name field is required.")
                },
                new object[]
                {
                GetValidPlayerInputModel().CloneWith(x =>
                {
                    x.Email = "joris@joris.com";
                    x.EmailValidate = "joris@joris.nl";
                }),
                new KeyValuePair<string, string>("EmailValidate", "'EmailValidate' and 'Email' do not match.")
            }
            };
            return testData;
        }

        private static PlayerCreateModel GetValidPlayerInputModel()
        {
            return new PlayerCreateModel()
            {
                Name = "Joris",
                Email = "joris@joris.nl",
                EmailValidate = "joris@joris.nl"
            };
        }

        [Fact]
        public async Task UpdatePlayer_CorrectPlayerNameAndId_UpdatedPlayerName()
        {
            string name = "Updated Player Name";
            int playerId = 1;
            var player = await PutRequestAsync<Player>($"/Player/{playerId}", new PlayerUpdateModel()
            {
                Id = playerId,
                Name = name,
                Email = "updateEmail@updateEmail",
                EmailValidate = "updateEmail@updateEmail"
            });

            player.Id.Should().Be(1);
            player.Name.Should().Be(name);
        }
    }
}