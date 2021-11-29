using Microsoft.NetConf2021.Maui.Models.Responses;

namespace Microsoft.NetConf2021.Maui.Models;

public class Category
{
    public Category(CategoryResponse response)
    {
        Id = response.Id;
        Genre = response.Genre;
    }   

    public Guid Id { get; set; }

    public string Genre { get; set; }
}
