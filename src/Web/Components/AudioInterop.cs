using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Podcast.Components;

public sealed class AudioInterop : IAudioInterop , IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public AudioInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", "./_content/Podcast.Components/audioJsInterop.js").AsTask());
    }

    public async Task PlayAsync(ElementReference element)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("play", element);
    }

    public async Task PauseAsync(ElementReference element)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("pause", element);
    }

    public async Task StopAsync(ElementReference element)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("stop", element);
    }

    public async Task SetMutedAsync(ElementReference element, bool value)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("setMuted", element, value);
    }

    public async Task SetVolumeAsync(ElementReference element, int value)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("setVolume", element, value / 100d);
    }

    public async Task SetCurrentTimeAsync(ElementReference element, double value)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("setCurrentTime", element, value);
    }

    public async Task SetPlaybackRateAsync(ElementReference element, double value)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("setPlaybackRate", element, value);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    public void SetUri(string? audioURI)
    {
        //no action is necessary
    }
}
