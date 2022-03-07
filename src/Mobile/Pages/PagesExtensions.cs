namespace Microsoft.NetConf2021.Maui.Pages
{
    public static class PagesExtensions
    {
        public static MauiAppBuilder ConfigurePages(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<DiscoverPage>();
            builder.Services.AddSingleton<SubscriptionsPage>();
            builder.Services.AddSingleton<ListenLaterPage>();
            builder.Services.AddSingleton<ListenTogetherPage>();
            builder.Services.AddSingleton<SettingsPage>();

            return builder;
        }
    }
}
