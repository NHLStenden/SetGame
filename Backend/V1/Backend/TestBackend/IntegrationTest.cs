using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Backend;
using Backend.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;




namespace TestBackend
{
    public class IntegrationTest : IDisposable
    {
        private TestServer _server;
        private HttpClient _client;
        
        public IntegrationTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .ConfigureTestServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(SeedService));
                    services.Remove(descriptor);
                    services.AddSingleton<SeedService, TestSeedService>();
                })
                .UseStartup<Startup>()
            );
            _client = _server.CreateClient();
        }
        
        protected async Task<T> GetRequest<T>(string apiPath, dynamic parameters)
        {
            if (apiPath.StartsWith("/"))
            {
                apiPath = apiPath.Substring(1);
            }

            var requestUri = $"http://localhost:5000/{apiPath}";
            bool firstParameter = true;
            foreach (var parameter in parameters.GetType().GetProperties())
            {
                var propertyName = parameter.Name;
                var propretyValue = parameters.GetType().GetProperty(propertyName).GetValue(parameters, null);

                if (firstParameter)
                {
                    
                    requestUri += "?";
                }

                if (!firstParameter)
                {
                    requestUri += "&";
                }

                requestUri += $"{propertyName}={propretyValue}";
                
                firstParameter = false;
            }
            
            var response = await _client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var objects = JsonConvert.DeserializeObject<T>(json);
            return objects;
        }

        public void Dispose()
        {
            _client?.Dispose();
            _server?.Dispose();
        }
    }
}