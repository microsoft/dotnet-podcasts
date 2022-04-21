using Podcast.Infrastructure.Data;
using Podcast.Infrastructure.Data.Models;

namespace Podcast.Infrastructure.Http.Feeds;

public interface IFeedClient
{
    Task<Show> GetShowAsync(Feed feed, CancellationToken cancellationToken);
    Task AddFeedAsync(PodcastDbContext podcastDbContext, string url, IReadOnlyCollection<string> feedCategories, CancellationToken cancellationToken);
}