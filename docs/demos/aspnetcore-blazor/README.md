### Web (ASP.NET Core Razor & Blazor)

The ASP.NET Core and Blazor apps are setup to run locally against the APIs running in Docker. If you are unable to deploy locally, you can use these pre-deployed services:

Open up ` src/Web/Server/appsettings.Development.json` and `src/Web/Client/wwwroot/appsettings.Development.json` enter this for both

```json
"PodcastApi": {
"BaseAddress": "https://podcastapica.ashyhill-df3dfdf5.eastus.azurecontainerapps.io"
},
"ListenTogetherHub": "https://dotnetpodcasts-listentogether-hub.azurewebsites.net/listentogether"
```
Watch the .NET Conf 2021 demo of .NET MAUI here: https://youtu.be/gYQxBHjRNr0?t=2699

### Demo Script Snippets

```razor
<ul>
    @foreach (var item in showSubscriptions)
    {
        <li>
            <h2>@item.Title</h2>
            <h3>@item.Author</h3>
            <img src="@item.Image" />
        </li>
    }
</ul>
```

```razor
<ShowCard Id="@item.Id" Title="@item.Title" Author="@item.Author" Image="@item.Image" />
```

```razor
<Grid Items="@showSubscriptions" TItem="ShowInfo">
    <ItemTemplate Context="item">
        <NavLink @key="item.Id" href="@($"show/{item.Id}")">
            <ShowCard Id="@item.Id" Title="@item.Title" Author="@item.Author" Image="@item.Image" />
        </NavLink>
    </ItemTemplate>
    <EmptyResults>
    </EmptyResults>
</Grid>
```

```razor
<NoResults Message="You haven’t subscribed to any channel yet."
        Description="Discover content according to your interests."
        Image="_content/Podcast.Pages/images/no-subscriptions.png" />
```

  
