using Microsoft.AspNetCore.Components;
using Podcast.Components;

namespace NetPodsMauiBlazor.Services;

internal class AudioInteropService : IAudioInterop
{
    private readonly INativeAudioService _nativeAudioService;
    private string url;

    public AudioInteropService(INativeAudioService nativeAudioService)
    {
        _nativeAudioService = nativeAudioService;
    }

    public ValueTask DisposeAsync()
    {
        return new ValueTask();
    }

    public Task Pause(ElementReference element)
    {
        return _nativeAudioService.PauseAsync();
    }

    public async Task Play(ElementReference element)
    {
        await _nativeAudioService.PlayAsync(_nativeAudioService.CurrentPosition);
    }

    public async Task SetCurrentTime(ElementReference element, double value)
    {
        await _nativeAudioService.PlayAsync(value);
    }

    public Task SetMuted(ElementReference element, bool value)
    {
        return _nativeAudioService.SetMuted(value);
    }

    public void SetUri(string audioURI)
    {
        if (audioURI != null)
        {
            url = audioURI;
            _nativeAudioService.InitializeAsync(audioURI).Wait();
        }
    }

    public Task SetVolume(ElementReference element, int value)
    {
        return _nativeAudioService.SetVolume(value);
    }

    public Task Stop(ElementReference element)
    {
        return _nativeAudioService.PauseAsync();
    }
}
