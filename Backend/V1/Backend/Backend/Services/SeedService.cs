using System;
using System.Collections.Generic;
using System.IO;
using Backend.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Backend.Services
{
    public abstract class SeedService
    {
        protected abstract string GetDataPath();
        
        public void Seed(SetContext db)
        {
            // var joris = new PlayerService().CreatePlayer("Joris");
            //     var game0 = new GameService().StartNewGame(joris.PlayerId);
            //     var game1 = new GameService().StartNewGame(joris.PlayerId);
            //
            // var martin = new PlayerService().CreatePlayer("Martin");
            //     var game3 = new GameService().StartNewGame(martin.PlayerId);
            //     var game4 = new GameService().StartNewGame(martin.PlayerId);
            //
            // string yaml = ConvertToYaml(new List<Game>() {game0});
            //var currentDirectory = Environment.CurrentDirectory;

            string yaml = File.ReadAllText(GetDataPath());
            List<Game> games = YamlToObject<List<Game>>(yaml);
            
            db.Games = games;
        }

        public static T YamlToObject<T>(string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            
            var result = deserializer.Deserialize<T>(yaml);
            return result;
        }
        
        public static string ConvertToYaml<T>(T obj)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yaml = serializer.Serialize(obj);
            return yaml;
        }
    }

    public class TestSeedService : SeedService
    {
        protected override string GetDataPath()
        {
            string path = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "TestData",
                "GameControllerTestData.yaml");
            return path;
        }
    }

    public class NormalSeedService : SeedService
    {
        protected override string GetDataPath()
        {
            string path = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, "TestData",
                "GameData.yaml");
            return path;
        }
    }
}