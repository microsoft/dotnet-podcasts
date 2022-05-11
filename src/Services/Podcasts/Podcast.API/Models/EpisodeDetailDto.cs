namespace Podcast.API.Models;

public sealed record EpisodeDetailDto(
    Guid Id,
    string Title,
    DateTime Published,
    string Url,
    string Description,
    string? Duration);