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
        _logger.LogInformation($"The show {title} at {url} was received by the ingestion worker.");
        var isExistingShow = await _podcastDbContext.Feeds.AnyAsync(feed => feed.Url.ToLower() == url.ToLower(), stoppingToken);
        if (isExistingShow) 
            return;

        _logger.LogInformation($"The show {title} doesn't already exist in the database, checking for accepted topics in the category list.");
        var isAcceptedTopic = AcceptedTopics.Any(topic => title.Contains(topic, StringComparison.InvariantCultureIgnoreCase));
        if (!isAcceptedTopic)
        {
            _logger.LogInformation($"The show {title} at {url} was not automatically approved.");
            var categoryStringArray = string.Empty;
            foreach (var cat in feedCategories)
            {
                categoryStringArray += $"{cat},";
            }

            if(categoryStringArray.Contains(","))
            {
                categoryStringArray = categoryStringArray.Substring(0, categoryStringArray.LastIndexOf(','));
            }

            // Must be manually approved
            var userFeed = new UserSubmittedFeed(title, url, categoryStringArray);
            await _podcastDbContext.UserSubmittedFeeds.AddAsync(userFeed, stoppingToken);
            await _podcastDbContext.SaveChangesAsync(stoppingToken);
            _logger.LogInformation($"The show {title} at {url} was saved as a user-submitted feed.");
            return;
        }

        await _feedClient.AddFeedAsync(_podcastDbContext, url, feedCategories, stoppingToken);

    }
}