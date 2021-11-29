using Podcast.Shared;

namespace Podcast.Pages.Models;

public record EpisodeInfo
{
    public Guid Id { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Url { get; init; } = default!;
    public DateTime Published { get; init; } = default!;
    public TimeSpan? Duration { get; init; }
    public ShowInfo Show { get; init; } = default!;

    public EpisodeInfo() { }

    public EpisodeInfo(Guid id, string title, string description, string url, DateTime published, TimeSpan? duration, ShowInfo show) =>
        (Id, Title, Description, Url, Published, Duration, Show) = (id, title, description, url, published, duration, show);

    public EpisodeInfo(Show show, Episode episode)
        : this(episode.Id, episode.Title, episode.Description, episode.Url, episode.Published, episode.Duration, new ShowInfo(show)) { }
}

