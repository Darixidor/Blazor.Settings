using Darixidor.Blazor.Settings.Extentions;
using Darixidor.Blazor.Settings.Interfaces;
using Darixidor.Blazor.Settings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Darixidor.Blazor.Settings.Controllers
{
    [Route("api/[controller]")]
    [ApiController][GenericControllerNameConvention]
    internal class SystemController<T> : ControllerBase
    {
        private readonly IAppSettings<T> appSettings;
        private IConfigurationRoot configurationRoot;

        public SystemController(IAppSettings<T> appSettings, IConfigurationRoot configurationRoot)
        {
            this.appSettings = appSettings;
            this.configurationRoot = configurationRoot;
        }

        [HttpGet("settings/enviroment")]
        public async Task<IActionResult> GetEnviroment()
        {
            return await Task.FromResult(new OkObjectResult(await this.appSettings.GetEnviroment()));
        }

        [HttpGet("settings/client")]
        public async Task<IActionResult> GetClientSettings()
        {
            var enviro = await this.appSettings.GetEnviroment();
            if (enviro == "prod")
            {
                return await Task.FromResult(new NotFoundResult());
            }

            var expando = this.configurationRoot.GetSettingsObject("client");
            return await Task.FromResult(new OkObjectResult(expando));
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    internal class GenericControllerNameConvention : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.GetGenericTypeDefinition() !=
                typeof(SystemController<>))
            {
                // Not a GenericController, ignore.
                return;
            }

            var entityType = controller.ControllerType.GenericTypeArguments[0];
            controller.ControllerName = "system";
        }
    }
}
