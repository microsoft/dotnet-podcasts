namespace Microsoft.NetConf2021.Maui.Services;

public interface IAudioService
{
    Task InitializeAsync(string audioURI);
    Task PlayAsync(double position = 0);
    Task PauseAsync();
    bool IsPlaying {  get; }
    double CurrentPosition {  get; }
}
