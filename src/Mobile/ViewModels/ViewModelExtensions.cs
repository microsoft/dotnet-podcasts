namespace Microsoft.NetConf2021.Maui.ViewModels;

public static class ViewModelExtensions
{
    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<CategoriesViewModel>();
        builder.Services.AddSingleton<CategoryViewModel>();
        builder.Services.AddSingleton<DiscoverViewModel>();
        builder.Services.AddSingleton<EpisodeDetailViewModel>();
        builder.Services.AddSingleton<EpisodeViewModel>();
        builder.Services.AddSingleton<ListenLaterViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddSingleton<ShellViewModel>();
        builder.Services.AddSingleton<ShowDetailViewModel>();
        builder.Services.AddSingleton<ShowViewModel>();
        builder.Services.AddSingleton<SubscriptionsViewModel>();

        return builder;
    }
}
