using Microsoft.AspNetCore.Components.WebView.Maui;
using MonkeyCache.FileStore;

namespace Microsoft.NetConf2021.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureEssentials()
            .ConfigureServices()
            .ConfigurePages()
            .ConfigureViewModels()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Segoe-Ui-Bold.ttf", "SegoeUiBold");
                fonts.AddFont("Segoe-Ui-Regular.ttf", "SegoeUiRegular");
                fonts.AddFont("Segoe-Ui-Semibold.ttf", "SegoeUiSemibold");
                fonts.AddFont("Segoe-Ui-Semilight.ttf", "SegoeUiSemilight");
            });

        Barrel.ApplicationId = "dotnetpodcasts";

        builder.Services.AddTransient<CategoriesPage>();
        builder.Services.AddTransient<CategoryPage>();
        builder.Services.AddTransient<DiscoverPage>();
        builder.Services.AddTransient<EpisodeDetailPage>();
        builder.Services.AddTransient<ListenLaterPage>();
        builder.Services.AddTransient<ListenTogetherPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<ShowDetailPage>();
        builder.Services.AddTransient<SubscriptionsPage>();

        return builder.Build();
    }
}
