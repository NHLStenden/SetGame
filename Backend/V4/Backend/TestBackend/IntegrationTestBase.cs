using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Backend;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace TestBackend
{
    public class IntegrationTest : IDisposable
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;

        public IntegrationTest()
        {
            Server = new TestServer(new WebHostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json");
                })
                .ConfigureTestServices(services =>
                {
                    
                    var descriptorDb = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(DbContextOptions<SetContext>));
                    services.Remove(descriptorDb);
                    services.AddDbContext<SetContext>(builder =>
                    {
                        builder.UseMySQL("Server=localhost;Database=SetGame;Uid=root;Pwd=Test@1234!;");
                    });
                    
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                             typeof(ISeedService));
                    services.Remove(descriptor);
                    services.AddSingleton<ISeedService, TestSeedService>();
                })
                .UseStartup<Startup>()
            );
            Client = Server.CreateClient();
        }
        
        
        
        protected async Task<T> GetRequestAsync<T>(string apiPath, dynamic parameters = null, bool ensureStatusCode = false)
        {
            var requestUri = CreateRequestUri(apiPath, parameters);

            var response = await Client.GetAsync(requestUri);

            if (ensureStatusCode)
            {
                response.EnsureSuccessStatusCode();
            }

            string json = await response.Content.ReadAsStringAsync();
            
            var objects = JsonConvert.DeserializeObject<T>(json);
            return objects;
        }
        
        protected async Task<T> PostRequestAsync<T>(string apiPath, Object objectToSendWithRequest = null, bool ensureStatusCodes = true, dynamic parameters = null)
        {
            var jsonString = JsonConvert.SerializeObject(objectToSendWithRequest);
            
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            
            var requestUri = CreateRequestUri(apiPath, parameters);

            var response = await Client.PostAsync(requestUri, content);

            if (ensureStatusCodes)
            {
                response.EnsureSuccessStatusCode();    
            }
            
            string json = await response.Content.ReadAsStringAsync();
            
            var objects = JsonConvert.DeserializeObject<T>(json);
            return objects;
        }
        
        protected async Task<T> PutRequestAsync<T>(string apiPath, Object objectToSendWithRequest, bool ensureStatusCodes = true, dynamic parameters = null)
        {
            var jsonString = JsonConvert.SerializeObject(objectToSendWithRequest);
            
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            
            var requestUri = CreateRequestUri(apiPath, parameters);

            var response = await Client.PutAsync(requestUri, content);

            if (ensureStatusCodes)
            {
                response.EnsureSuccessStatusCode();    
            }

            string json = await response.Content.ReadAsStringAsync();
            
            var objects = JsonConvert.DeserializeObject<T>(json);
            return objects;
        }

        private static string CreateRequestUri(string apiPath, dynamic parameters)
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
                    var propertyValue = parameters.GetType().GetProperty(propertyName).GetValue(parameters, null);

                    if (firstParameter)
                    {
                        requestUri += "?";
                    }

                    if (!firstParameter)
                    {
                        requestUri += "&";
                    }

                    if (propertyValue is IEnumerable)
                    {
                        var queryParameterParts = (propertyValue as IEnumerable).Cast<object>()
                            .Select(item => $"{propertyName}={item}");

                        requestUri += string.Join("&", queryParameterParts);
                    }
                    else
                    {
                        requestUri += $"{propertyName}={propertyValue}";
                    }

                    firstParameter = false;
                }
            }

            return requestUri;
        }

        public void Dispose()
        {
            Client?.Dispose();
            Server?.Dispose();
        }
    }
}