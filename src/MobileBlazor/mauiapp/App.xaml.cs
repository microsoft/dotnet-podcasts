using Application = Microsoft.Maui.Controls.Application;

namespace NetPodsMauiBlazor;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}
