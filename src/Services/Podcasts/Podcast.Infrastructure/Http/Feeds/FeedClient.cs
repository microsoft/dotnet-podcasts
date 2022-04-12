using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Podcast.Infrastructure.Data;
using Podcast.Infrastructure.Data.Models;

namespace Podcast.Infrastructure.Http.Feeds;

public class FeedClient : IFeedClient
{
    private static readonly XmlSerializer XmlSerializer = new(typeof(Rss));
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly ILogger<FeedClient> _logger;

    public FeedClient(HttpClient httpClient, IConfiguration configuration, ILogger<FeedClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Show> GetShowAsync(Feed feed, CancellationToken cancellationToken)
    {
        await using var feedContent = await _httpClient.GetStreamAsync(feed.Url, cancellationToken);
        var rss = (Rss)XmlSerializer.Deserialize(feedContent)!;
        var imagesStorage = _configuration["Storage:Images"];
        var updatedShow = Mapper.Map(rss, imagesStorage);
        return updatedShow;
    }

    public async Task AddFeedAsync(PodcastDbContext podcastDbContext, string url, IReadOnlyCollection<string> feedCategories, CancellationToken cancellationToken)
    {
        var feed = new Feed(Guid.NewGuid(), url);

        var categories = await podcastDbContext.Categories
            .Where(category => feedCategories.Any(feedCategory => feedCategory == category.Genre))
            .ToListAsync(cancellationToken);

        foreach (var category in categories)
            feed.Categories.Add(new FeedCategory(category.Id, feed.Id));

        try
        {
            var show = await GetShowAsync(feed, cancellationToken);
            feed.Show = show;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error adding feed: {error}", ex.Message);
        }

        await podcastDbContext.Feeds.AddAsync(feed, cancellationToken);
        await podcastDbContext.SaveChangesAsync(cancellationToken);
    }
}