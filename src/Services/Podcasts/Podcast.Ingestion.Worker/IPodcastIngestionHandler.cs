namespace Podcast.Ingestion.Worker;

public interface IPodcastIngestionHandler
{
    Task HandleIngestionAsync(string title, string url, IReadOnlyCollection<string> feedCategories,
        CancellationToken stoppingToken);
}
