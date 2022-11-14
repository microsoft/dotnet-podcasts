using Application = Microsoft.Maui.Controls.Application;

namespace Microsoft.NetConf2021.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        TheTheme.SetTheme();

        if (Config.Desktop)
            MainPage = new DesktopShell();
        else
            MainPage = new MobileShell();

        Routing.RegisterRoute(nameof(DiscoverPage), typeof(DiscoverPage));
        Routing.RegisterRoute(nameof(ShowDetailPage), typeof(ShowDetailPage));
        Routing.RegisterRoute(nameof(EpisodeDetailPage), typeof(EpisodeDetailPage));
        Routing.RegisterRoute(nameof(CategoriesPage), typeof(CategoriesPage));
        Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
    }
}
