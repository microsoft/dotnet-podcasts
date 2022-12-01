namespace Podcast.API.Routes;

public static class FeedsApi
{
    public static RouteGroupBuilder MapFeedsApi(this RouteGroupBuilder group)
    {
        group.MapPost("/", CreateFeed).WithName("CreateFeed");
        group.MapGet("/", GetAllFeeds).WithName("GetFeeds");
        group.MapPut("/{id}", UpdateFeed)
            .RequireAuthorization("modify_feeds")
            .AddOpenApiSecurityRequirement()
            .WithName("UpdateFeedById");
        group.MapDelete("/{id}", DeleteFeed)
            .RequireAuthorization("modify_feeds")
            .AddOpenApiSecurityRequirement()
            .WithName("DeleteFeedById");
        return group;
    }

    private static RouteHandlerBuilder AddOpenApiSecurityRequirement(this RouteHandlerBuilder builder)
    {
        var scheme = new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Name = JwtBearerDefaults.AuthenticationScheme,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new()
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        };
        builder.WithOpenApi(operation => new(operation)
        {
            Security =
            {
                new()
                {
                    [scheme] = new List<string>()
                }
            }
        });
        return builder;
    }

    public static async ValueTask CreateFeed(QueueClient queueClient, UserSubmittedFeedDto feed, CancellationToken cancellationToken)
    {
        await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        await queueClient.SendMessageAsync(new BinaryData(feed), cancellationToken: cancellationToken);
    }

    public static async ValueTask<Ok<List<UserSubmittedFeed>>> GetAllFeeds(PodcastDbContext podcastDbContext, CancellationToken cancellationToken)
    {
        var feeds = await podcastDbContext.UserSubmittedFeeds.OrderByDescending(f => f.Timestamp).ToListAsync(cancellationToken);
        return TypedResults.Ok(feeds);
    }

    public static async ValueTask<Results<NotFound, Accepted>> UpdateFeed(PodcastDbContext podcastDbContext, IFeedClient feedClient, Guid id, CancellationToken cancellationToken)
    {
        var feed = podcastDbContext.UserSubmittedFeeds.Find(id);
        if (feed is null)
            return TypedResults.NotFound();

        var categories = feed.Categories.Split(',');

        await feedClient.AddFeedAsync(podcastDbContext, feed.Url, categories, cancellationToken);
        podcastDbContext.Remove(feed);
        await podcastDbContext.SaveChangesAsync(cancellationToken);
        return TypedResults.Accepted($"/feeds/{id}");
    }

    public static async ValueTask<Results<NotFound, NoContent>> DeleteFeed(PodcastDbContext podcastDbContext, IFeedClient feedClient, Guid id, CancellationToken cancellationToken)
    {
        var feed = podcastDbContext.UserSubmittedFeeds.FirstOrDefault(x => x.Id == id);
        if (feed is null)
            return TypedResults.NotFound();

        podcastDbContext.Remove(feed);
        await podcastDbContext.SaveChangesAsync(cancellationToken);
        return TypedResults.NoContent();
    }
}
