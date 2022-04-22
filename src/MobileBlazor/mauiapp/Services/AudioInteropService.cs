using SharedMauiLib;
using Microsoft.AspNetCore.Components;
using Podcast.Components;
using Podcast.Pages.Data;
using System.Timers;

namespace NetPodsMauiBlazor.Services;

internal class AudioInteropService : IAudioInterop
{
    private readonly INativeAudioService _nativeAudioService;
    private readonly PlayerService _playerService;
    private readonly System.Timers.Timer currentTimeTimer;

    public AudioInteropService(
        INativeAudioService nativeAudioService,
        PlayerService playerService)
    {
        _nativeAudioService = nativeAudioService;
        _playerService = playerService;

        currentTimeTimer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
        currentTimeTimer.Elapsed += OnCurrentTimeEvent;
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
        await _nativeAudioService.SetCurrentTime(value);
    }

    public Task SetMuted(ElementReference element, bool value)
    {
        return _nativeAudioService.SetMuted(value);
    }

    public void SetUri(string audioURI)
    {
        if (audioURI != null)
        {
            _nativeAudioService.InitializeAsync(audioURI).Wait();
            currentTimeTimer.Start();
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

    public ValueTask DisposeAsync()
    {
        currentTimeTimer.Dispose();
        return _nativeAudioService.DisposeAsync();
    }

    private void OnCurrentTimeEvent(Object source, ElapsedEventArgs e)
    {
        _playerService.CurrentTime = _nativeAudioService.CurrentPosition;
    }
}
