using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Podcast.API.Controllers;
using Podcast.API.Models;
using Podcast.Infrastructure.Data;
using Podcast.Infrastructure.Http.Feeds;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PodcastDb");
builder.Services.AddSqlServer<PodcastDbContext>(connectionString);

var queueConnectionString = builder.Configuration.GetConnectionString("FeedQueue");
builder.Services.AddSingleton(new QueueClient(queueConnectionString, "feed-queue"));
builder.Services.AddHttpClient<IFeedClient, FeedClient>();

builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1",
        new OpenApiInfo { Description = "NetPodcast API", Title = ".NetConf2021", Version = "v1" });
});
builder.Services.AddCors(setup =>
{
    setup.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

await EnsureDbAsync(app.Services);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetPodcast Api v1");
});

app.UseCors();

app.MapGet("v1/categories",
    async (PodcastDbContext podcastDbContext, CancellationToken cancellationToken) =>
    {
        var categories = await podcastDbContext.Categories.Select(x => new CategoryDto(x.Id, x.Genre))
            .ToListAsync(cancellationToken);
        return categories;
    });

app.MapGet("v1/episodes/{id}", async (PodcastDbContext podcastDbContext, Guid id,
    CancellationToken cancellationToken) =>
{
    var episode = await podcastDbContext.Episodes.Include(episode => episode.Show)
        .Where(episode => episode.Id == id)
        .Select(episode => new EpisodeDto(episode))
        .FirstAsync(cancellationToken);
    return episode;
});

var feedIngestionEnabled = app.Configuration.GetValue<bool>("Features:FeedIngestion");

if (feedIngestionEnabled)
{
    app.MapFeedEndpointRoutes();   
}

app.MapControllers();
app.Run();

static async Task EnsureDbAsync(IServiceProvider sp)
{
    await using var db = sp.CreateScope().ServiceProvider.GetRequiredService<PodcastDbContext>();
    await db.Database.MigrateAsync();
}