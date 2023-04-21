using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Podcast.Components;
using Podcast.Pages.Data;
using Podcast.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient<PodcastService>(
    client => {
        client.BaseAddress = new Uri(builder.Configuration["REACT_APP_API_BASE_URL"]!);
        client.DefaultRequestHeaders.Add("api-version", "1.0");
    });
builder.Services.AddScoped<ThemeInterop>();
builder.Services.AddScoped<IAudioInterop, AudioInterop>();
builder.Services.AddScoped<LocalStorageInterop>();
builder.Services.AddScoped<ClipboardInterop>();
builder.Services.AddScoped<SubscriptionsService>();
builder.Services.AddScoped<ListenLaterService>();
builder.Services.AddSingleton<PlayerService>();
builder.Services.AddScoped<ListenTogetherHubClient>(_ =>
    new ListenTogetherHubClient(builder.Configuration["REACT_LISTEN_TOGETHER_HUB"]!));

await builder.Build().RunAsync();