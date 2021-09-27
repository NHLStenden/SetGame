using System.IO;
using Backend.Models;
using Backend.Repository;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Backend.Services
{
    public abstract class SeedService : ISeedService
    {
        public abstract string GetDataPath();

        public virtual void Seed(SetContext db, IGameService gameService, IPlayerRepository playerRepository)
        {
            string yamlInput = File.ReadAllText(GetDataPath());
            Game game = YamlToObject<Game>(yamlInput);
            db.Games.Add(game);
            db.SaveChanges();
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