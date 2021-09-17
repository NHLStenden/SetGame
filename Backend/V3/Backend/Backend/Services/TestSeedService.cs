using System;
using System.IO;

namespace Backend.Services
{
    public class TestSeedService : SeedService
    {
        public override string GetDataPath()
        {
            string path = Path.Combine(new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName, "TestData",
                "GameControllerTestData.yaml");
            return path;
        }
    }
}