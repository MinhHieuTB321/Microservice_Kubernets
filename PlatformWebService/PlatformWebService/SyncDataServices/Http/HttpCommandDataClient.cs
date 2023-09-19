using PlatformWebService.DTOs;
using System.Text;
using System.Text.Json;

namespace PlatformWebService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient=httpClient;
            _config=configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDtos platform)
        {
            var httpContent= new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync($"{_config["CommandService"]}/api/c/Platforms", httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was Ok!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService was Not Ok!");
            }
        }
    }
}
