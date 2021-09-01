using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend;
using Backend.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

using NUnit.Framework;


namespace TestBackend
{
    public class DeckControllerTests
    {
        private TestServer _server;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Test]
        public async Task GetNewBoard()
        {
            var response = await _client.GetAsync("http://localhost:5000/Deck/GetNewDeck");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var deck = JsonConvert.DeserializeObject<Deck>(json);

            deck.DeckId.Should().Be(1);
            deck.Cards.Count.Should().Be(81);
        }

        [Test]
        public async Task GetCardsFromDeck_DeckDoesNotExists_ThrowException()
        {
            var response = await _client.GetAsync("http://localhost:5000/Deck/GetCardsFromDeck?deckId=0&numberOfCards=3");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        } 
        
        [Test]
        public async Task GetCardsFrom_Deck_ThreeCards()
        {
            var response = await _client.GetAsync("http://localhost:5000/Deck/GetNewDeck");
            
            response = await _client.GetAsync("http://localhost:5000/Deck/GetCardsFromDeck?deckId=1&numberOfCards=3");

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var cards = JsonConvert.DeserializeObject<Card[]>(json);

            cards.Should().HaveCount(3);
            cards.Should().OnlyHaveUniqueItems();
            
            var expectedCards = new Card[]
            {
                new Card() {
                    Color = Backend.Models.Color.Violet,
                    Fill = Backend.Models.Fill.Striped,
                    NrOfShapes = 3,
                    Shape = Shape.Diamond
                }, new Card()
                {
                    Color = Backend.Models.Color.Violet,
                    Fill = Backend.Models.Fill.Striped,
                    NrOfShapes = 1,
                    Shape = Shape.Diamond
                }, new Card()
                {
                    Color = Color.Red,
                    Fill = Backend.Models.Fill.Hollow,
                    NrOfShapes = 3,
                    Shape = Shape.Wave                    
                }
            };

            cards.Should().BeEquivalentTo(expectedCards);



        }
    }
}