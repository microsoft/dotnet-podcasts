# .NET MAUI podcast app for Android, iOS, macOS, and Windows

Before getting started ensure that you have [.NET MAUI installed with Visual Studio 2022](https://docs.microsoft.com/dotnet/maui/get-started/installation).

The .sln is located in **/src/Mobile/Podcasts.DotnetMaui.sln**
The startup project is **Microsoft.NetConf2021.Maui**
Before building the app we need to configure some variables:

This solution has two projects:

- **Microsoft.NetConf2021.Maui**: Maui App based on default template of .NET Maui Blazor App.
- References to Blazor project
  - **[src/Web/Components/Podcast.Components.Maui.csproj](/src/Web/Components/Podcast.Components.Maui.csproj)**: Razor Class Library project with shared components.

# Configuration

In Config.cs you can find:

```csharp
public static string BaseWeb = $"{Base}:5002/listentogether";
public static string Base = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "http://localhost";
public static string APIUrl = $"{Base}:5003/";
public static string ListenTogetherUrl = $"{Base}:5001/listentogether";
```

Adjust the urls and ports to speak to local or deploy web services.

## ListenTogeherIsVisible property

Setting to true the app will show the listen together tab on mobile and FlyoutItem in desktop elements.

## Urls

One the Web and Api are available we will need to update next properties:

- **BaseWeb**: Full url where web has been deployed
- **BaseAPI**: Full url where API has been deployed
- **ListenTogetherUrl**: Url where listen together app has been deployed.

## Running on macOS and iOS

Via CLI it is necesary to build:

- `dotnet build src/Web/Components/Podcast.Components.Maui.csproj`
- Mac: `dotnet build src/Mobile/Microsoft.NetConf2021.Maui.csproj -t:run -f net7.0-maccatalyst`
- iOS: `dotnet build -t:Run -f net7.0-ios -p:\_DeviceName=:v2:udid='[EMULATOR ID]'`

To run iOS from windows we can use Visual Studio 2022 Preview from Windows.

