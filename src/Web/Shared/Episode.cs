namespace Podcast.Shared;

public readonly record struct Episode(
    Guid Id,
    string Title,
    string Description,
    string Explicit,
    DateTime Published,
    TimeSpan? Duration,
    string Url);
