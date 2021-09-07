using System;
using System.Collections.Generic;
using System.IO;
using Backend.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Backend.Services
{
    public class SeedService
    {
        public static void Seed(SetContext db)
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
            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var testDataPath = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "TestData",
                "GameControllerTestData.yaml");
            
            
            string currentDirectory = Environment.CurrentDirectory;
            var game = deserializer.Deserialize<List<Game>>(File.ReadAllText(testDataPath));

            db.Games = game;
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
}