using Podcast.Components;
using Podcast.Pages.Models;

namespace Podcast.Pages.Data;

public class SubscriptionsService
{
    private const string ShowSubscriptionsKey = "ShowSubscriptions";
    private readonly LocalStorageInterop _localStorage;

    private List<ShowInfo> _shows = default!;
    private bool _isInitialized = false;
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

    public event Action<IEnumerable<ShowInfo>>? SubscriptionsChanged;

    public SubscriptionsService(LocalStorageInterop localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task Initialize()
    {
        if (!_isInitialized)
        {
            await _semaphore.WaitAsync();
            if (!_isInitialized)
            {
                var subscriptions = await _localStorage.GetItem<ShowInfo[]>(ShowSubscriptionsKey);
                _shows = subscriptions?.ToList() ?? new List<ShowInfo>();
                _isInitialized = true;
            }
            _semaphore.Release();
        }
    }

    public async Task<IEnumerable<ShowInfo>> GetShowSubscriptions()
    {
        await Initialize();
        return _shows;
    }

    public async Task<bool> IsSubscribedShow(Guid showId)
    {
        await Initialize();
        return _shows?.Any(s => s.Id == showId) ?? false;
    }

    public Task ToggleShowSubscription(ShowInfo show, bool isSubscribed) =>
        isSubscribed ? SubscribeShow(show) : UnsubscribeShow(show);

    public async Task SubscribeShow(ShowInfo show)
    {
        await Initialize();
        if (!_shows.Any(s => s.Id == show.Id))
        {
            _shows.Add(show);
            await _localStorage.SetItem(ShowSubscriptionsKey, _shows);
            SubscriptionsChanged?.Invoke(_shows);
        }
    }

    public async Task UnsubscribeShow(ShowInfo show)
    {
        await Initialize();
        var subscription = _shows.FirstOrDefault(s => s.Id == show.Id);
        if (subscription != null)
        {
            _shows.Remove(subscription);
            await _localStorage.SetItem(ShowSubscriptionsKey, _shows);
            SubscriptionsChanged?.Invoke(_shows);
        }
    }
}
