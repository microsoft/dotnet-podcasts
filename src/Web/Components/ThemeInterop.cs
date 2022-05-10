using Microsoft.JSInterop;

namespace Podcast.Components;

public sealed class ThemeInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    
    public Func<Theme, Task>? SystemThemeChanged;

    public ThemeInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
           "import", "./_content/Podcast.Components/themeJsInterop.js").AsTask());
    }

    public ValueTask<Theme> GetThemeAsync() => 
        GetThemeByIdentifierAsync("getTheme");

    public ValueTask<Theme> GetSystemThemeAsync() => 
        GetThemeByIdentifierAsync("getSystemTheme");

    public async ValueTask RegisterForSystemThemeChangedAsync()
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync(
            "registerForSystemThemeChanged",
            DotNetObjectReference.Create(this),
            nameof(OnSystemThemeChanged));
    }
    
    [JSInvokable]
    public Task OnSystemThemeChanged(bool isDarkTheme) => 
        SystemThemeChanged?.Invoke(
            isDarkTheme ? Theme.Dark : Theme.Light)
        ?? Task.CompletedTask;

    private async ValueTask<Theme> GetThemeByIdentifierAsync(string identifier)
    {
        var module = await _moduleTask.Value;
        var theme = await module.InvokeAsync<string>(identifier);
        return theme != null ? (Theme)Enum.Parse(typeof(Theme), theme) : Theme.Light;
    }

    public async Task SetThemeAsync(Theme theme)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("setTheme", theme.ToString());
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}