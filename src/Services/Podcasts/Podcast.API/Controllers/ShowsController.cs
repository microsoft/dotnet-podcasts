using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Podcast.API.Models;
using Podcast.Infrastructure.Data;

namespace Podcast.API.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly PodcastDbContext _podcastDbContext;

    public ShowsController(PodcastDbContext podcastDbContext)
    {
        _podcastDbContext = podcastDbContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ShowDto>))]
    public async Task<IEnumerable<ShowDto>> GetAsync(int limit, string? term,
        Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var showsQuery = _podcastDbContext.Shows.Include(show => show.Feed!.Categories)
            .ThenInclude(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(term))
            showsQuery = showsQuery.Where(show => show.Title.Contains(term));

        if (categoryId is not null)
            showsQuery = showsQuery.Where(show =>
                show.Feed!.Categories.Any(y => y.CategoryId == categoryId));

        var shows = await showsQuery.OrderByDescending(show => show.Feed!.IsFeatured)
            .ThenBy(x => x.Title)
            .Take(limit)
            .Select(x => new ShowDto(x))
            .ToListAsync(cancellationToken);
        return shows;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShowDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShowDto>> GetByIdAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var show = await _podcastDbContext.Shows
            .Include(show => show.Episodes.OrderByDescending(episode => episode.Published))
            .Include(show => show.Feed!.Categories)
            .ThenInclude(feed => feed.Category)
            .Where(x => x.Id == id)
            .Select(show => new ShowDto(show))
            .FirstOrDefaultAsync(cancellationToken);
        return show == null ? NotFound() : Ok(show);
    }
}