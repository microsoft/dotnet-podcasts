using Podcast.Components;
using Podcast.Pages.Models;

namespace Podcast.Pages.Data;

public class ListenLaterService
{
    private const string ListenLaterKey = "ListenLater";
    private readonly LocalStorageInterop _localStorage;
    private readonly SemaphoreSlim _semaphore = new(1);

    private HashSet<EpisodeInfo> _episodes = default!;
    private bool _isInitialized = false;

    public event Action<ICollection<EpisodeInfo>>? EpisodesChanged;

    public ListenLaterService(LocalStorageInterop localStorage)
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
                    var episodes = await _localStorage.GetItem<EpisodeInfo[]>(ListenLaterKey);
                    _episodes = episodes?.ToHashSet() ?? new();
                    _isInitialized = true;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public async Task<ICollection<EpisodeInfo>> GetEpisodesAsync()
    {
        await InitializeAsync();
        return _episodes;
    }

    public async Task<bool> IsListenLaterEpisodeAsync(Guid episodeId)
    {
        await InitializeAsync();
        return _episodes?.Any(s => s.Id == episodeId) ?? false;
    }

    public Task ToggleListenLaterEpisodeAsync(EpisodeInfo episode, bool isListenLater) =>
        isListenLater ? AddListenLaterEpisodeAsync(episode) : RemoveListenLaterEpisodeAsync(episode.Id);

    public async Task AddListenLaterEpisodeAsync(EpisodeInfo episode)
    {
        await InitializeAsync();
        if (!_episodes.Any(s => s.Id == episode.Id))
        {
            _episodes.Add(episode);
            await _localStorage.SetItem(ListenLaterKey, _episodes);
            EpisodesChanged?.Invoke(_episodes);
        }
    }

    public async Task RemoveListenLaterEpisodeAsync(Guid episodeId)
    {
        await InitializeAsync();
        var episode = _episodes.FirstOrDefault(s => s.Id == episodeId);
        if (episode != null)
        {
            _episodes.Remove(episode);
            await _localStorage.SetItem(ListenLaterKey, _episodes);
            EpisodesChanged?.Invoke(_episodes);
        }
    }
}
