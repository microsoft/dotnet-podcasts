using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using MonkeyCache.FileStore;

namespace Microsoft.NetConf2021.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var appBuilder = MauiApp.CreateBuilder();
        appBuilder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureEssentials()
            .ConfigureServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Segoe-Ui-Bold.ttf", "SegoeUiBold");
                fonts.AddFont("Segoe-Ui-Regular.ttf", "SegoeUiRegular");
                fonts.AddFont("Segoe-Ui-Semibold.ttf", "SegoeUiSemibold");
                fonts.AddFont("Segoe-Ui-Semilight.ttf", "SegoeUiSemilight");
            });

        Barrel.ApplicationId = "dotnetpodcasts";

        return appBuilder.Build();
    }
}

