using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

using Xunit;

namespace TestBackend
{
    [Collection("IntegrationTests")]
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
            PlayerCreateDto inputCreatePlayerDto = new ()
            {
                Name = "Joris", Email = "test@test.com", EmailValidate = "test@test.com", Password = "Test@1234!",
                PasswordValidate = "Test@1234!",
                Roles = new List<string>()
                {
                    "User"
                }
            };

            // var expectedPlayer = new Player()
            // {
            //     Name = "Joris", Email = "test@test.com"
            // };
            
            var player = await PostRequestAsync<Player>($"/Player", inputCreatePlayerDto);

            player.Name.Should().Be("Joris");
            player.Email.Should().Be("test@test.com");
        }

        [Theory]
        [MemberData(nameof(GetInvalidInputAndProblemDetails))]
        public async Task CreatePlayer_InvalidInput_BadRequest(PlayerCreateDto playerCreateDto, KeyValuePair<string, string> validator)
        {
            var response = await Client.PostAsJsonAsync("/Player", playerCreateDto);

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

        private static PlayerCreateDto GetValidPlayerInputModel()
        {
            return new PlayerCreateDto()
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
            var player = await PutRequestAsync<Player>($"/Player/{playerId}", new PlayerUpdateDto()
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