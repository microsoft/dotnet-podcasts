### Blazor Hybrid + .NET MAUI Setup

Open `src/MobileBlazor/mauiapp/MauiProgram.cs` and enter

```csharp
public static string BaseWeb = $"https://dotnetpodcasts.azurewebsites.net/";
public static string APIUrl = $"https://podcastapica.delightfulocean-02c18c32.canadacentral.azurecontainerapps.io/v1/";
public static string ListenTogetherUrl = $"https://dotnetpodcasts-listentogether-hub.azurewebsites.net/listentogether";
```

Watch the .NET Conf 2021 demo of Blaozry Hybrid & .NET MAUI here: https://youtu.be/gYQxBHjRNr0?t=3999
