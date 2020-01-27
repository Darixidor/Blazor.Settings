using Darixidor.Blazor.Settings.Interfaces;
using Darixidor.Blazor.Settings.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Darixidor.Blazor.Settings.Services
{
    internal class ClientSettingsService<T> : IAppSettings<T>
    {
        private readonly ILogger<ClientSettingsService<T>> logger;
        private readonly HttpClient httpClient;

        public ClientSettingsService(ILogger<ClientSettingsService<T>> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public async Task<string> GetEnviroment()
        {
            var clientEnviroment = await httpClient.GetStringAsync("api/system/settings/enviroment");
            logger.LogDebug(clientEnviroment);
            return await Task.FromResult(clientEnviroment);
        }

        public async Task<T> GetSettings()
        {
            var clientConfig = await httpClient.GetStringAsync("api/system/settings/client");
            logger.LogDebug(clientConfig);
            var settings = JsonSerializer.Deserialize<Settings<T>>(clientConfig, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return await Task.FromResult(settings.Data);
        }
    }
}
