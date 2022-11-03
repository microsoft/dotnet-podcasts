using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podcast.Infrastructure.Http
{
    public class JitterHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Who would do such a thing?!
            // TODO fix this horrible perf leak!
            await Task.Delay(TimeSpan.FromSeconds(Random.Shared.NextInt64(1, 11)));
            return await base.SendAsync(request, cancellationToken);
        }
    }

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
}
