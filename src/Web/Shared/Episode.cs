namespace Podcast.Shared;

public record Episode(
    Guid Id,
    string Title,
    string Description,
    string Explicit,
    DateTime Published,
    TimeSpan? Duration,
    string Url);
