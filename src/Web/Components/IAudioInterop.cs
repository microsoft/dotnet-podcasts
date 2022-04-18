using Microsoft.AspNetCore.Components;

namespace Podcast.Components;

public interface IAudioInterop
{
    void SetUri(string audioURI);

    Task Play(ElementReference element);

    Task Pause(ElementReference element);

    Task Stop(ElementReference element);

    Task SetMuted(ElementReference element, bool value);

    Task SetVolume(ElementReference element, int value);

    Task SetCurrentTime(ElementReference element, double value);

    ValueTask DisposeAsync();
}
