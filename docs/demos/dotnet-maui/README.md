## .NET MAUI Configuration

The .NET MAUI apps are setup to run locally against the APIs running in Docker. If you are unable to deploy locally, you can use these pre-deployed services:

Open `src/Mobile/Config.cs` and enter

```csharp
public static string BaseWeb = $"https://dotnetpodcasts.azurewebsites.net/";
public static string APIUrl = $"https://podcastapica.ashyhill-df3dfdf5.eastus.azurecontainerapps.io/";
public static string ListenTogetherUrl = $"https://dotnetpodcasts-listentogether-hub.azurewebsites.net/listentogether";
```

Watch the .NET Conf 2021 demo of .NET MAUI here: https://youtu.be/gYQxBHjRNr0?t=3357
