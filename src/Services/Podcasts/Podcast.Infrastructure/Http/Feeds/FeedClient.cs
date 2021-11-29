using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Podcast.Infrastructure.Data.Models;

namespace Podcast.Infrastructure.Http.Feeds;

public class FeedClient : IFeedClient
{
    private static readonly XmlSerializer XmlSerializer = new(typeof(Rss));
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public FeedClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<Show> GetShowAsync(Feed feed, CancellationToken cancellationToken)
    {
        await using var feedContent = await _httpClient.GetStreamAsync(feed.Url, cancellationToken);
        var rss = (Rss)XmlSerializer.Deserialize(feedContent)!;
        var imagesStorage = _configuration["Storage:Images"];
        var updatedShow = Mapper.Map(rss, imagesStorage);
        return updatedShow;
    }
}