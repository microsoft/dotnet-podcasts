using Microsoft.AspNetCore.Components;

namespace Podcast.Components;

public interface IAudioInterop
{
    void SetUri(string? audioURI);

    Task PlayAsync(ElementReference element);

    Task PauseAsync(ElementReference element);

    Task StopAsync(ElementReference element);

    Task SetMutedAsync(ElementReference element, bool value);

    Task SetVolumeAsync(ElementReference element, int value);

    Task SetCurrentTimeAsync(ElementReference element, double value);

    Task SetPlaybackRateAsync(ElementReference element, double value);

    ValueTask DisposeAsync();
}
