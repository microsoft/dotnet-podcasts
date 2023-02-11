using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podcast.Infrastructure.Http;

public class ShowClient
{
    private readonly HttpClient _httpClient;

    public ShowClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CheckLink(string showLink)
    {
        var response = await _httpClient.GetAsync(showLink, HttpCompletionOption.ResponseHeadersRead);
        return response.IsSuccessStatusCode;
    }
}