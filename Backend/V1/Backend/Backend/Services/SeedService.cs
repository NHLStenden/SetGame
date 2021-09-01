using Backend.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Backend.Services
{
    public class SeedService
    {
        public static void Seed()
        {
            var joris = new PlayerService().CreatePlayer("Joris");
                var game0 = new GameService().StartNewGame(joris.PlayerId);
                var game1 = new GameService().StartNewGame(joris.PlayerId);
            
            var martin = new PlayerService().CreatePlayer("Martin");
                var game3 = new GameService().StartNewGame(martin.PlayerId);
                var game4 = new GameService().StartNewGame(martin.PlayerId);

                ConvertToYaml(game0);
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