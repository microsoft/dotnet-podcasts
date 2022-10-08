namespace Podcast.Components;

public record RoomEpisode(
    Guid Id,
    string Title,
    string Description,
    string Url,
    string Image,
    DateTime Published,
    TimeSpan? Duration,
    RoomShow Show);
