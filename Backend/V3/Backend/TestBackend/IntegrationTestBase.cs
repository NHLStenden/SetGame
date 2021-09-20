using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Backend;
using Backend.Models;
using Backend.Services;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

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
                    
                    var descriptorDb = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<SetContext>));
                    services.Remove(descriptorDb);
                    services.AddDbContext<SetContext>((options, context) =>
                    {
                        context.UseMySQL("Server=localhost;Database=SetGame;Uid=root;Pwd=Test@1234!;");
                    });
                    
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(ISeedService));
                    services.Remove(descriptor);
                    services.AddSingleton<ISeedService, TestSeedService>();
                })
                .UseStartup<Startup>()
            );
            _client = _server.CreateClient();
        }
        
        protected async Task<T> GetRequest<T>(string apiPath, dynamic parameters = null)
        {
            if (apiPath.StartsWith("/"))
            {
                apiPath = apiPath.Substring(1);
            }

            var requestUri = $"http://localhost:5000/{apiPath}";
            if (parameters != null)
            {
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
            }
            
            var response = await _client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            // JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions()
            // {
            //     ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            // };

            
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