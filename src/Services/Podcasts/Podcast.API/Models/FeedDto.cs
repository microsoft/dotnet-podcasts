using System.Collections.Generic;

namespace Podcast.API.Models;

public record FeedDto(string Title, string Url, List<string> Categories);