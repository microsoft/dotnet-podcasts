using Podcast.Components;
using Podcast.Pages.Models;

namespace Podcast.Pages.Data;

public class ListenLaterService
{
    private const string ListenLaterKey = "ListenLater";
    private readonly LocalStorageInterop _localStorage;

    private List<EpisodeInfo> _episodes = default!;
    private bool _isInitialized = false;
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

    public event Action<ICollection<EpisodeInfo>>? EpisodesChanged;

    public ListenLaterService(LocalStorageInterop localStorage)
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
                var episodes = await _localStorage.GetItem<EpisodeInfo[]>(ListenLaterKey);
                _episodes = episodes?.ToList() ?? new List<EpisodeInfo>();
                _isInitialized = true;
            }
            _semaphore.Release();
        }
    }

    public async Task<ICollection<EpisodeInfo>> GetEpisodes()
    {
        await Initialize();
        return _episodes;
    }

    public async Task<bool> IsListenLaterEpisode(Guid episodeId)
    {
        await Initialize();
        return _episodes?.Any(s => s.Id == episodeId) ?? false;
    }

    public Task ToggleListenLaterEpisode(EpisodeInfo episode, bool isListenLater) =>
        isListenLater ? AddListenLaterEpisode(episode) : RemoveListenLaterEpisode(episode.Id);

    public async Task AddListenLaterEpisode(EpisodeInfo episode)
    {
        await Initialize();
        if (!_episodes.Any(s => s.Id == episode.Id))
        {
            _episodes.Add(episode);
            await _localStorage.SetItem(ListenLaterKey, _episodes);
            EpisodesChanged?.Invoke(_episodes);
        }
    }

    public async Task RemoveListenLaterEpisode(Guid episodeId)
    {
        await Initialize();
        var episode = _episodes.FirstOrDefault(s => s.Id == episodeId);
        if (episode != null)
        {
            _episodes.Remove(episode);
            await _localStorage.SetItem(ListenLaterKey, _episodes);
            EpisodesChanged?.Invoke(_episodes);
        }
    }
}
