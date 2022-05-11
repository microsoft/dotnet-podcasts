namespace Podcast.Components;

public readonly record struct RoomEpisode(
    Guid Id,
    string Title,
    string Description,
    string Url,
    DateTime Published,
    TimeSpan? Duration,
    RoomShow Show);
