
namespace Podcast.Infrastructure.Queues.Messages;
public record NewFeedRequested(string Title, string Url, List<string> Categories);