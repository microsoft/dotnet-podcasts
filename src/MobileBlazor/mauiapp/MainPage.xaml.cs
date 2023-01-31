namespace NetPodsMauiBlazor;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        SharedMauiLib.MediaElementAudioService.MediaElement = mediaElement;
    }
}
