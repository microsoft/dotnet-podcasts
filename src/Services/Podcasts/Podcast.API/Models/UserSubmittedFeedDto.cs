namespace Podcast.API.Models;

public record UserSubmittedFeedDto(Guid Id, string Title, string Url, List<string> Categories);