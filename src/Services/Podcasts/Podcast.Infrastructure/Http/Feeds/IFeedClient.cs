using Podcast.Infrastructure.Data.Models;

namespace Podcast.Infrastructure.Http.Feeds;

public interface IFeedClient
{
    Task<Show> GetShowAsync(Feed feed, CancellationToken cancellationToken);
}