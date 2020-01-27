using Darixidor.Blazor.Settings.Controllers;
using Darixidor.Blazor.Settings.Interfaces;
using Darixidor.Blazor.Settings.Services;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Darixidor.Blazor.Settings.Extentions
{
    public static class ServerExtentions
    {
        public static void AddServerSettingsService<TSettingsType>(this IServiceCollection services)
        {
            services.AddTransient<IAppSettings<TSettingsType>, ServerSettingsService<TSettingsType>>();
        }

        //public static void AddSettingsController<TSettingsType>(this IMvcBuilder mvcBuilder)
        //{
        //    mvcBuilder.AddApplicationPart(Assembly.GetAssembly(typeof(ServerExtentions))).AddControllersAsServices();
        //}
    }



    public class SettingsControllerFeatureProvider<TSettingsType> : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            // There's no 'real' controller for this entity, so add the generic version.
            var controllerType = typeof(SystemController<>)
                        .MakeGenericType(typeof(TSettingsType)).GetTypeInfo();
            feature.Controllers.Add(controllerType);
        }
    }
}
