using System;
using System.IO;
using Backend.Models;

namespace Backend.Services
{
    public class TestSeedService : SeedService
    {
        public override string GetDataPath()
        {
            string path = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName,
                "TestData",
                "GameControllerTestData.yaml");
            return path;
        }

        public Game GetGame()
        {
            string yaml = File.ReadAllText(GetDataPath());
            var game = YamlToObject<Game>(yaml);
            return game;
        }
    }
}