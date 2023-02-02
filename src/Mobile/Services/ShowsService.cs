using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.NetConf2021.Maui.Models.Responses;
using MonkeyCache.FileStore;

namespace Microsoft.NetConf2021.Maui.Services;

public class ShowsService
{
    private readonly HttpClient httpClient;
    private readonly ListenLaterService listenLaterService;
    private bool firstLoad = true;

    public ShowsService(ListenLaterService listenLaterService)
    {
        this.httpClient = new HttpClient() { BaseAddress = new Uri(Config.APIUrl) };
        httpClient.DefaultRequestHeaders.Add("api-version", "1.0");
        this.listenLaterService = listenLaterService;
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
        var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=20&categoryId={idCategory}");

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
        var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=20&categoryId={idCategory}&term={term}");

        return showsResponse?.Select(response => GetShow(response));
    }

    public async Task<IEnumerable<Show>> SearchShowsAsync(string term)
    {
        var showsResponse = await TryGetAsync<IEnumerable<ShowResponse>>($"shows?limit=20&term={term}");

        return showsResponse?.Select(response => GetShow(response));
    }

    private Show GetShow(ShowResponse response)
    {
        return new Show(response, listenLaterService);
    }

    private Task<T> TryGetAsync<T>(string path)
    {
        if (firstLoad)
        {
            firstLoad = false;

            // On first load, it takes a significant amount of time to initialize
            // the ShowsService. For example, Connectivity.NetworkAccess, Barrel.Current.Get,
            // and HttpClient all take time to initialize.
            //
            // Don't block the UI thread while doing this initialization, so the app starts faster.
            // Instead, run the first TryGet in a background thread to unblock the UI during startup.
            return Task.Run(() => TryGetImplementationAsync<T>(path));
        }

        return TryGetImplementationAsync<T>(path);
    }

    private async Task<T> TryGetImplementationAsync<T>(string path)
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
                responseData = JsonSerializer.Deserialize<T>(json);
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
