using System.Net.Http.Json;
using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;

namespace ListenTogether.Infrastructure.Http;

public class EpisodesHttpClient : IEpisodesClient
{
    private readonly HttpClient _httpClient;

    public EpisodesHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Episode> GetEpisodeByIdAsync(Guid episodeId, CancellationToken cancellationToken)
    {
        return (await _httpClient.GetFromJsonAsync<Episode>($"episodes/{episodeId}", cancellationToken))!;
    }
}