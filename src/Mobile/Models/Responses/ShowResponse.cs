namespace Microsoft.NetConf2021.Maui.Models.Responses;

public class ShowResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Description { get; set; }

    public Uri Image { get; set; }

    public DateTime Updated { get; set; }

    public Uri Link { get; set; }

    public string Email { get; set; }

    public string Language { get; set; }

    public bool IsFeatured { get; set; }
    
    public CategoryResponse[] Categories { get; set; }

    public EpisodeResponse[] Episodes { get; set; }
}


