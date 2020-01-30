# Darixidor.Blaxor.Settings
[![Nuget](https://img.shields.io/nuget/v/Darixidor.Blazor.Settings?style=for-the-badge)](https://www.nuget.org/packages/Darixidor.Blazor.Settings)

[![Nuget](https://img.shields.io/nuget/dt/Darixidor.Blazor.Settings?style=for-the-badge)](https://www.nuget.org/packages/Darixidor.Blazor.Settings)

[![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg?style=for-the-badge)](https://github.com/Darixidor/Blazor.Settings/graphs/commit-activity)

I have been working with Blazor Client side for a few weeks now and ran into an issue: trying to access my appsettings.json file
from my client side code doesn't work and for good reason: your app settigns files are on the server, your client code has no idea about them.
I do NOT want to maintain 2 sets of appsettings files, so I created this library. 

# How to Use
## appsettings.json Files
This is an example of how to setup your appsettings files. You need to have two sections, one for server settings and the other for app settings.

```
{
  "server": {
    "setting1": "ServerTest",
    "setting2": "ServerTest"
  },

  "client": {
    "setting1": "ClientTest",
    "setting2": "ClientTest"
  }
}
```
You MUST have a section called 'server' and a section called 'client' for the services to both work properly.

## Settings Models
You should create two classes that will allow the data from your appsettings to be deserialized into by the serices. One accessible to the client
code and one accessible to the server code. These classes don't need to be shared between them. 
Examples:
```
// SERVER MODEL
namespace Blazor.Settings.Clientside.Server.Models
{
    public class ServerSettings
    {
        public string Setting1 { get; set; }

        public string Setting2 { get; set; }
    }
}

// CLIENT MODEL
namespace Blazor.Settings.Clientside.Client.Model
{
    public class ClientSettings
    {
        public string Setting1 { get; set;}

        public string Setting2 { get; set; }
    }
}
``` 

## Client Setup
Client setup is easy: a one line registration in the  `Startup` classes ConfigureServices method:

```
services.AddClientSettingsService<ClientSettings>(); // type parameter is your settings models you created above
```


## Server Setup
Server setup isn't as easy as the client setup, but it is easy. It comes in 2 parts: a registration of the `IAppSetting<T>` 
service that the server can use to access the settings from the `server` section of the app settings file and a second that registers a controller 
that exposes the settings AND the enviroment variable to the client (api/system/settings/enviroment and api/system/settings/client routes.). 

```
 services.AddServerSettingsService<ServerSettings>();// type parameter is your settings models you created above
 services.AddMvc().ConfigureApplicationPartManager(p =>
      p.FeatureProviders.Add(new SettingsControllerFeatureProvider<ServerSettings>())); // adds the system controller to your api that the
                                                                                        // client service needs to funtion.
```


# Usage
In either the server or client code, dependency inject a `IAppSetting<T>` (where T is the settings model you created above):

## Client
```
@page "/"
@inject IAppSettings<ClientSettings> settingsService

Current enviroment @enviroment
Setting @clientSettings.Setting1 and @clientSettings.Setting2


@code{
    private string enviroment;
    private ClientSettings clientSettings
    protected override async Task OnInitializedAsync()
    {
        enviroment = await settingsService.GetEnviroment();
        clientSettings = await settingsService.GetSettings();
    }
}

```
The `IAppSetting<T>` implementation for the client side makes API calls to the System controller that is registered in the 
`SettingsControllerFeatureProvider<T>` on the server. 

## Server
Now because you have direct access ti the `ICongifurartionRoot` through dependency injection, you don't *have* to use this library on 
the server... but if your doing it in one place, why not do it in both!
```
    public class ValuesController : ControllerBase
    {
        private readonly IAppSetting<ServerSettings> severSettingsService;

        public ValuesController(IAppSetting<ServerSettings> severSettingsService)
        {
            this.severSettingsService = severSettingsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var settings = severSettingsService.GetSettings();
            var environment = severSettingsService.GetEnviroment();

            return Ok(environment);
        }
    }
```


# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

