using System.Net.Http.Json;
using Microsoft.NetConf2021.Maui.Models.Responses;
using MonkeyCache.FileStore;
using Newtonsoft.Json;

namespace Microsoft.NetConf2021.Maui.Services;

public class ShowsService
{
    private readonly HttpClient httpClient;

    public ShowsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        var categoryResponse = await TryGetAsync<IEnumerable<CategoryResponse>>("categories");
        return categoryResponse?.Select(response => new Category(response));
    }

    public async Task<Show> GetShowByIdAsync(Guid id)
    {
        var showResponse = await TryGetAsync<ShowResponse>($"shows/{id}");

        return showResponse == null
            ? null
            : GetShow(showResponse);
    }

    public Task<IEnumerable<Show>> GetShowsAsync()
    {
        return SearchShowsAsync(string.Empty);
    }

    public async Task<IEnumerable<Show>> GetShowsByCategoryAsync(Guid idCategory)
    {
        var result = new List<Show>();
        var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=10&categoryId={idCategory}");

        if (showsResponse == null)
            return result;
        else
        {
            foreach(var show in showsResponse)
            {
                result.Add(GetShow(show));
            }

            return result;
        }
    }

    public async Task<IEnumerable<Show>> SearchShowsAsync(Guid idCategory, string term)
    {
        var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=10&categoryId={idCategory}&term={term}");

        return showsResponse?.Select(response => GetShow(response));
    }

    public async Task<IEnumerable<Show>> SearchShowsAsync(string term)
    {
        var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=10&term={term}");

        return showsResponse?.Select(response => GetShow(response));
    }

    private Show GetShow(ShowResponse response)
    {
        return new Show(response);
    }

    private async Task<T> TryGetAsync<T>(string path)
    {
        var json = string.Empty;

#if !MACCATALYST
        if (Connectivity.NetworkAccess == NetworkAccess.None)
            json = Barrel.Current.Get<string>(path);
#endif
        if (!Barrel.Current.IsExpired(path))
            json = Barrel.Current.Get<string>(path);

        T responseData = default;
        try
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                var response = await httpClient.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadFromJsonAsync<T>();
                }
            }
            else
            {
                responseData = JsonConvert.DeserializeObject<T>(json);
            }

            if (responseData != null)
                Barrel.Current.Add(path, responseData, TimeSpan.FromMinutes(10));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }

        return responseData;
    }
}
