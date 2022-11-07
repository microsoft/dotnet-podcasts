using Application = Microsoft.Maui.Controls.Application;

namespace Microsoft.NetConf2021.Maui;

public partial class App : Application
{
    public static bool WindowCreated { get; private set; }
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

    protected override Window CreateWindow(IActivationState activationState)
    {


        var window = new MauiWindow(MainPage);

        window.Created += Window_Created;
        return window;
    }


    private void Window_Created(object sender, EventArgs e)
    {
        WindowCreated = true;
    }
}
