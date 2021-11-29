using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Infrastructure;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Hosting;
using Podcast.Components;
using Podcast.Pages.Data;
using Podcast.Shared;
using System;

namespace NetPodsMauiBlazor
{
    public static class MauiProgram
    {
        public static string BaseWeb = $"{Base}:5002/listentogether";
        public static string Base = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2" : "http://localhost";
        public static string APIUrl = $"{Base}:5000/v1/";
        public static string ListenTogetherUrl = $"{Base}:5001/listentogether";

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .RegisterBlazorMauiWebView()
                .UseMauiApp<App>();

            builder.Services.AddBlazorWebView();
            builder.Services.AddHttpClient<PodcastService>(client =>
            {
                client.BaseAddress = new Uri(APIUrl);
            });
            builder.Services.AddScoped<ThemeInterop>();
            builder.Services.AddScoped<AudioInterop>();
            builder.Services.AddScoped<LocalStorageInterop>();
            builder.Services.AddScoped<ClipboardInterop>();
            builder.Services.AddScoped<SubscriptionsService>();
            builder.Services.AddScoped<ListenLaterService>();
            builder.Services.AddSingleton<PlayerService>();
            builder.Services.AddScoped<ListenTogetherHubClient>(_ =>
                new ListenTogetherHubClient(ListenTogetherUrl));
            builder.Services.AddScoped<ComponentStatePersistenceManager>();
            builder.Services.AddScoped<PersistentComponentState>(sp => sp.GetRequiredService<ComponentStatePersistenceManager>().State);

            return builder.Build();
        }
    }
}