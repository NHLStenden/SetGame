using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Repository;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Backend.Services
{
    public abstract class SeedService : ISeedService
    {
        public abstract string GetDataPath();

        public void Seed(SetContext db, IGameService gameService, IPlayerRepository playerRepository)
        {
            string yamlInput = File.ReadAllText(GetDataPath());
            Game game = YamlToObject<Game>(yamlInput);
            db.Games.Add(game);
            db.SaveChanges();


            // //
            // //
            // var player = new Player()
            // {
            //     Name = "Joris"
            // };
            // var b = playerRepository.AddAsync(player).Result;
            //
            // var result = gameService.StartNewGame(player.Id).Result;
            //
            // var gameYaml = ConvertToYaml(result);


            //
            // db.Games.Add(game);
            // db.SaveChanges();

            // db.Games.AddRange(games);re
            // db.SaveChanges();
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
}