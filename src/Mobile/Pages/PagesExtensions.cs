namespace Microsoft.NetConf2021.Maui.Pages;

public static class PagesExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
    {
        // main tabs of the app
        builder.Services.AddSingleton<DiscoverPage>();
        builder.Services.AddSingleton<SubscriptionsPage>();
        builder.Services.AddSingleton<ListenLaterPage>();
        builder.Services.AddSingleton<ListenTogetherPage>();
        builder.Services.AddSingleton<SettingsPage>();

        // pages that are navigated to
        builder.Services.AddTransient<CategoriesPage>();
        builder.Services.AddTransient<CategoryPage>();
        builder.Services.AddTransient<EpisodeDetailPage>();
        builder.Services.AddTransient<ShowDetailPage>();

        return builder;
    }
}
