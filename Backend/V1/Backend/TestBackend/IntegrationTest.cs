// using System.Dynamic;
// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Backend;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.TestHost;
// using Newtonsoft.Json;
//
//
//
//
// namespace TestBackend
// {
//     public class IntegrationTest
//     {
//         private TestServer _server;
//         private HttpClient _client;
//
//         [SetUp]
//         public void Setup()
//         {
//             _server = new TestServer(new WebHostBuilder()
//                 .UseStartup<Startup>()
//             );
//             _client = _server.CreateClient();
//         }
//         
//         protected async Task<T> GetRequest<T>(string apiPath, dynamic parameters)
//         {
//             if (apiPath.StartsWith("/"))
//             {
//                 apiPath = apiPath.Substring(1);
//             }
//
//             var requestUri = $"http://localhost:5000/{apiPath}";
//             bool firstParameter = true;
//             foreach (var parameter in parameters.GetType().GetProperties())
//             {
//                 var propertyName = parameter.Name;
//                 var propretyValue = parameters.GetType().GetProperty(propertyName).GetValue(parameters, null);
//
//                 if (firstParameter)
//                 {
//                     
//                     requestUri += "?";
//                 }
//
//                 if (!firstParameter)
//                 {
//                     requestUri += "&";
//                 }
//
//                 requestUri += $"{propertyName}={propretyValue}";
//                 
//                 firstParameter = false;
//             }
//             
//             var response = await _client.GetAsync(requestUri);
//
//             response.EnsureSuccessStatusCode();
//
//             string json = await response.Content.ReadAsStringAsync();
//
//             var objects = JsonConvert.DeserializeObject<T>(json);
//             return objects;
//         }
//
//         [TearDown]
//         public void TearDown()
//         {
//             _client.Dispose();
//             _server.Dispose();
//         }
//     }
// }