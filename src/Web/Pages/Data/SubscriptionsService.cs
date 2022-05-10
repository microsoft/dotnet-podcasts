using Podcast.Components;
using Podcast.Pages.Models;

namespace Podcast.Pages.Data;

public class SubscriptionsService
{
    private const string ShowSubscriptionsKey = "ShowSubscriptions";
    private readonly LocalStorageInterop _localStorage;
    private readonly SemaphoreSlim _semaphore = new(1);

    private HashSet<ShowInfo> _shows = default!;
    private bool _isInitialized = false;

    public event Action<IEnumerable<ShowInfo>>? SubscriptionsChanged;

    public SubscriptionsService(LocalStorageInterop localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            try
            {
                await _semaphore.WaitAsync();
                if (!_isInitialized)
                {
                    var subscriptions = await _localStorage.GetItem<ShowInfo[]>(ShowSubscriptionsKey);
                    _shows = subscriptions?.ToHashSet() ?? new();
                    _isInitialized = true;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public async Task<IEnumerable<ShowInfo>> GetShowSubscriptionsAsync()
    {
        await InitializeAsync();
        return _shows;
    }

    public async Task<bool> IsSubscribedShowAsync(Guid showId)
    {
        await InitializeAsync();
        return _shows?.Any(s => s.Id == showId) ?? false;
    }

    public Task ToggleShowSubscriptionAsync(ShowInfo show, bool isSubscribed) =>
        isSubscribed ? SubscribeShowAsync(show) : UnsubscribeShowAsync(show);

    public async Task SubscribeShowAsync(ShowInfo show)
    {
        await InitializeAsync();
        if (!_shows.Any(s => s.Id == show.Id))
        {
            _shows.Add(show);
            await _localStorage.SetItem(ShowSubscriptionsKey, _shows);
            SubscriptionsChanged?.Invoke(_shows);
        }
    }

    public async Task UnsubscribeShowAsync(ShowInfo show)
    {
        await InitializeAsync();
        var subscription = _shows.FirstOrDefault(s => s.Id == show.Id);
        if (subscription != null)
        {
            _shows.Remove(subscription);
            await _localStorage.SetItem(ShowSubscriptionsKey, _shows);
            SubscriptionsChanged?.Invoke(_shows);
        }
    }
}
