namespace Podcast.Ingestion.Worker;

public interface IPodcastIngestionHandler
{
    Task HandleIngestionAsync(string title, string url, IReadOnlyCollection<string> feedCategories,
        CancellationToken stoppingToken);
}

public class PodcastIngestionHandler : IPodcastIngestionHandler
{
    private static readonly string[] AcceptedTopics =
    {
        ".NET", "C#", "F#", "ASP.NET", "MAUI", "Xamarin", "Blazor", "Microservices", "Visual Basic", "Visual Studio"
    };

    private readonly PodcastDbContext _podcastDbContext;
    private readonly IFeedClient _feedClient;
    private readonly ILogger<PodcastIngestionHandler> _logger;

    public PodcastIngestionHandler(PodcastDbContext podcastDbContext, IFeedClient feedClient, ILogger<PodcastIngestionHandler> logger)
    {
        _podcastDbContext = podcastDbContext;
        _feedClient = feedClient;
        _logger = logger;
    }

    public async Task HandleIngestionAsync(string title, string url, IReadOnlyCollection<string> feedCategories,
        CancellationToken stoppingToken)
    {
        var isAcceptedTopic = AcceptedTopics.Any(topic => title.Contains(topic, StringComparison.InvariantCultureIgnoreCase));
        if (!isAcceptedTopic) return;

        var isExistingShow = await _podcastDbContext.Shows.AnyAsync(show => show.Title == title, stoppingToken);
        if (isExistingShow) return;

        var feed = new Feed(Guid.NewGuid(), url);

        var categories = await _podcastDbContext.Categories
            .Where(category => feedCategories.Any(feedCategory => feedCategory == category.Genre))
            .ToListAsync(stoppingToken);

        foreach (var category in categories)
        {
            feed.Categories.Add(new FeedCategory(category.Id, feed.Id));
        }

        try
        {
            var show = await _feedClient.GetShowAsync(feed, stoppingToken);
            feed.Show = show;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error adding feed: {error}", ex.Message);
        }

        await _podcastDbContext.Feeds.AddAsync(feed, stoppingToken);
        await _podcastDbContext.SaveChangesAsync(stoppingToken);
    }
}