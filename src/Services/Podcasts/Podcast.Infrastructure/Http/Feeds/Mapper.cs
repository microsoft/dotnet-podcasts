using Podcast.Infrastructure.Data.Models;

namespace Podcast.Infrastructure.Http.Feeds;

internal static class Mapper
{
    internal static Show Map(Rss rss, string imagesStorage)
    {
        var imageUrl = (rss.Channel.Image?.Url ?? rss.Channel.Image2?.Href) ?? throw new ArgumentNullException(nameof(Image.Url));
        var image = string.IsNullOrEmpty(imagesStorage) ? imageUrl : GetImageStorageUrl(imagesStorage, rss.Channel.Title, imageUrl);
        var link = rss.Channel.Link?.Href ?? rss.Channel.Link2 ?? throw new ArgumentNullException(nameof(Link.Href));
        var author = rss.Channel.Author ?? string.Empty;
        var email = rss.Channel.Owner?.Email ?? string.Empty;
        var language = rss.Channel.Language ?? string.Empty;
        var pubDate = RssHelper.ConvertDateTime(rss.Channel.PubDate).GetValueOrDefault();
        var episodes = rss.Channel.Item.Where(item => item.Enclosure != null).Select(Map).ToList();

        var show = new Show(author, rss.Channel.Description, email, language, rss.Channel.Title,
            link, image, pubDate);
        episodes.ToList().ForEach(episode => show.Episodes.Add(episode));

        return show;
    }

    private static Episode Map(Item item)
    {
        var description = item.Summary ?? item.Description ?? throw new ArgumentNullException(nameof(Item.Description));
        var duration = RssHelper.ConvertDuration(item.Duration);
        var @explicit = !string.IsNullOrEmpty(item.Explicit) ? item.Explicit : "no";
        var pubDate = RssHelper.ConvertDateTime(item.PubDate).GetValueOrDefault();
        var url = item.Enclosure!.Url ?? throw new ArgumentNullException(nameof(Enclosure.Url));

        var episode = new Episode(description, duration, @explicit, pubDate, item.Title, url);
        return episode;
    }

    private static string GetImageStorageUrl(string imagesStorage, string showTitle, string showImage)
    {
        var imagePath = new UriBuilder(showImage).Path;
        var imageExtension = Path.GetExtension(imagePath);
        var imageUrl = $"{imagesStorage}{showTitle}{imageExtension ?? ".jpg"}";
        return imageUrl;
    }
}