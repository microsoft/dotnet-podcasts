using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Podcast.Shared;

namespace Podcast.Server.Pages
{
    public class Landing : PageModel
    {
        private readonly PodcastService _podcastService;

        public Show[]? FeaturedShows { get; set; }

        public Landing(PodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public async Task OnGet()
        {
            var shows = await _podcastService.GetShows(50, null);
            FeaturedShows = shows?.Where(s => s.IsFeatured).ToArray();
        }
    }
}
