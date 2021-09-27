using System;
using System.IO;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services
{
    public class GenerateYamlSeedService : SeedService
    {
        public override string GetDataPath()
        {
            throw new NotImplementedException();
        }

        public override void Seed(SetContext db, IGameService gameService, IPlayerRepository playerRepository)
        {
            var player = new Player()
            {
                Name = "Joris"
            };
            var b = playerRepository.AddAsync(player).Result;
            
            var result = gameService.StartNewGame(player.Id).Result;
            
            
            var gameYaml = ConvertToYaml(result);
            
            string pathTestData = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, "TestData",
                "GameData.yaml");
            
            File.WriteAllText(pathTestData, gameYaml);
            
            string pathGameControllerTestData = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).Parent.FullName, "TestBackend", "TestData",
                "GameControllerTestData.yaml");
            
            File.WriteAllText(pathGameControllerTestData, gameYaml);
        }
    }
}