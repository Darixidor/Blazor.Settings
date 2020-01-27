using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Darixidor.Blazor.Settings.Interfaces;
using Darixidor.Blazor.Settings.Extentions;
using Darixidor.Blazor.Settings.Models;

namespace Darixidor.Blazor.Settings.Services
{
    internal class ServerSettingsService<T> : IAppSettings<T>
    {
        private readonly ILogger<ServerSettingsService<T>> logger;
        private readonly IConfigurationRoot configurationRoot;

        public ServerSettingsService(ILogger<ServerSettingsService<T>> logger, IConfigurationRoot configurationRoot)
        {
            this.logger = logger;
            this.configurationRoot = configurationRoot;
        }

        public async Task<string> GetEnviroment()
        {
            return await Task.FromResult(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        public async Task<T> GetSettings()
        {
            var expando = configurationRoot.GetSettingsObject("server");
            var temp = System.Text.Json.JsonSerializer.Serialize(expando);
            logger.LogDebug(temp);
            var settings = JsonSerializer.Deserialize<Settings<T>>(temp, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return await Task.FromResult(settings.Data);
        }
    }
}
