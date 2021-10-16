using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(HttpClient httpClient , IConfiguration configuration )
        {
            _httpClient = httpClient;
            _config = configuration;
        }
        public async Task SendPlatformCommand(PlatformReadDto platformRead)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platformRead), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_config["CommandService"]}/api/c/Platforms/" ,httpContent );

            if(response.IsSuccessStatusCode)
                System.Console.WriteLine("Response is successful");
            else
                System.Console.WriteLine("Response is  unsuccessful");

        }
    }
}
