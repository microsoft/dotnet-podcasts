using Microsoft.EntityFrameworkCore;
using Podcast.Infrastructure.Data.Models;
using Podcast.Infrastructure.Http;
using Podcast.Infrastructure.Http.Feeds;

namespace Podcast.Infrastructure.Data;

public class PodcastDbContext : DbContext
{
    private readonly ShowClient _showClient;

    protected PodcastDbContext()
    {
    }

    public PodcastDbContext(DbContextOptions options, ShowClient showClient) : base(options)
    {
        _showClient = showClient;
    }

    public DbSet<Show> Shows => Set<Show>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Episode> Episodes => Set<Episode>();
    public DbSet<Feed> Feeds => Set<Feed>();
    public DbSet<UserSubmittedFeed> UserSubmittedFeeds => Set<UserSubmittedFeed>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feed>().HasData(Seed.Feeds);
        modelBuilder.Entity<Category>().HasData(Seed.Categories);
        modelBuilder.Entity<Show>().HasQueryFilter(show => _showClient.CheckLink(show.Link).Result);
        modelBuilder.Entity<FeedCategory>().HasData(Seed.FeedCategories);
        modelBuilder.Entity<FeedCategory>().HasKey(prop => new { prop.FeedId, prop.CategoryId });
        base.OnModelCreating(modelBuilder);
    }
}