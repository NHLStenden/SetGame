using System;
using System.IO;

namespace Backend.Services
{
    public class NormalSeedService : SeedService
    {
        public override string GetDataPath()
        {
            string path = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).FullName, "TestData",
                "GameData.yaml");
            return path;
        }
    }
}