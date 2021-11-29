using System.Xml.Serialization;

namespace Podcast.Infrastructure.Http.Feeds;

[XmlRoot(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
public class Link
{
    [XmlAttribute(AttributeName = "rel")]
    public string? Rel { get; set; }
    [XmlAttribute(AttributeName = "type")]
    public string? Type { get; set; }
    [XmlAttribute(AttributeName = "href")]
    public string? Href { get; set; }
    [XmlAttribute(AttributeName = "title")]
    public string? Title { get; set; }
}

[XmlRoot(ElementName = "image")]
public class Image
{
    [XmlElement(ElementName = "url")]
    public string? Url { get; set; }
    [XmlElement(ElementName = "title")]
    public string? Title { get; set; }
    [XmlElement(ElementName = "link")]
    public string? Link2 { get; set; }
}

[XmlRoot(ElementName = "image", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
public class Image2
{
    [XmlAttribute(AttributeName = "href")]
    public string? Href { get; set; }
}

[XmlRoot(ElementName = "owner", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
public class Owner
{
    [XmlElement(ElementName = "name", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Name { get; set; }
    [XmlElement(ElementName = "email", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Email { get; set; }
}

[XmlRoot(ElementName = "enclosure")]
public class Enclosure
{
    [XmlAttribute(AttributeName = "url")]
    public string? Url { get; set; }
    [XmlAttribute(AttributeName = "length")]
    public string? Length { get; set; }
    [XmlAttribute(AttributeName = "type")]
    public string? Type { get; set; }
}

[XmlRoot(ElementName = "item")]
public class Item
{
    [XmlElement(ElementName = "title")]
    public string Title { get; set; } = null!;
    [XmlElement(ElementName = "description")]
    public string? Description { get; set; }
    [XmlElement(ElementName = "pubDate")]
    public string? PubDate { get; set; }
    [XmlElement(ElementName = "author")]
    public List<string>? Author { get; set; }
    [XmlElement(ElementName = "enclosure")]
    public Enclosure? Enclosure { get; set; }
    [XmlElement(ElementName = "image", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public Image2? Image2 { get; set; }
    [XmlElement(ElementName = "duration", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Duration { get; set; }
    [XmlElement(ElementName = "summary", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Summary { get; set; }
    [XmlElement(ElementName = "subtitle", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Subtitle { get; set; }
    [XmlElement(ElementName = "keywords", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Keywords { get; set; }
    [XmlElement(ElementName = "explicit", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Explicit { get; set; }
    [XmlElement(ElementName = "episodeType", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? EpisodeType { get; set; }
    [XmlElement(ElementName = "episode", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Episode { get; set; }
}

[XmlRoot(ElementName = "channel")]
public class Channel
{
    [XmlElement(ElementName = "link", Namespace = "http://www.w3.org/2005/Atom")]
    public Link? Link { get; set; }
    [XmlElement(ElementName = "title")]
    public string Title { get; set; } = null!;
    [XmlElement(ElementName = "generator")]
    public string? Generator { get; set; }
    [XmlElement(ElementName = "description")]
    public string Description { get; set; } = null!;
    [XmlElement(ElementName = "copyright")]
    public string? Copyright { get; set; }
    [XmlElement(ElementName = "language")]
    public string? Language { get; set; }
    [XmlElement(ElementName = "pubDate")]
    public string? PubDate { get; set; }
    [XmlElement(ElementName = "link")]
    public string? Link2 { get; set; }
    [XmlElement(ElementName = "image")]
    public Image? Image { get; set; }
    [XmlElement(ElementName = "author", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Author { get; set; }
    [XmlElement(ElementName = "image", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public Image2? Image2 { get; set; }
    [XmlElement(ElementName = "summary", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Summary { get; set; }
    [XmlElement(ElementName = "subtitle", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public string? Subtitle { get; set; }
    [XmlElement(ElementName = "owner", Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd")]
    public Owner? Owner { get; set; }
    [XmlElement(ElementName = "item")] public List<Item> Item { get; set; } = new List<Item>();
}

[XmlRoot(ElementName = "rss")]
public class Rss
{
    [XmlElement(ElementName = "channel")]
    public Channel Channel { get; set; } = null!;
}
