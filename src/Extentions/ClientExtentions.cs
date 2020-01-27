using Darixidor.Blazor.Settings.Interfaces;
using Darixidor.Blazor.Settings.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darixidor.Blazor.Settings.Extentions
{
    public static class ClientExtentions
    {
        public static void AddClientSettingsService<TSettingsType>(this IServiceCollection services)
        {
            services.AddTransient<IAppSettings<TSettingsType>,ClientSettingsService<TSettingsType>>();
        }
    }
}
