namespace Podcast.Components;

public record RoomEpisode(
    Guid Id,
    string Title,
    string Description,
    string Url,
    DateTime Published,
    TimeSpan? Duration,
    RoomShow Show);
