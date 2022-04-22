using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Podcast.API.Models;
using Podcast.Infrastructure.Data;
using Podcast.Infrastructure.Http.Feeds;

namespace Microsoft.AspNetCore.Builder;

public static class FeedEndpointsWebApplicationExtensions
{
    public static void MapFeedEndpointRoutes(this WebApplication app)
    {
        // receive user-submitted feeds
        app.MapPost("v1/feeds", async (QueueClient queueClient, UserSubmittedFeedDto feed, CancellationToken cancellationToken) =>
        {
            await queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            await queueClient.SendMessageAsync(new BinaryData(feed), cancellationToken: cancellationToken);
        })
        .WithName("SubmitNewFeed")
        .WithTags("Feeds");

        // get all the user-submitted feeds
        app.MapGet("v1/feeds", (PodcastDbContext podcastDbContext, CancellationToken cancellationToken) =>
        {
            return podcastDbContext.UserSubmittedFeeds.OrderByDescending(f => f.Timestamp).ToListAsync(cancellationToken);
        })
        .WithName("GetUserSubmittedFeeds")
        .WithTags("Feeds");

        // update a user-submitted feed
        app.MapPut("v1/feeds/{id}", async (PodcastDbContext podcastDbContext, IFeedClient feedClient, Guid id, CancellationToken cancellationToken) =>
        {
            var feed = podcastDbContext.UserSubmittedFeeds.Find(id);
            if (feed is null)
                return;

            var categories = feed.Categories.Split(',');

            await feedClient.AddFeedAsync(podcastDbContext, feed.Url, categories, cancellationToken);
            podcastDbContext.Remove(feed);
            await podcastDbContext.SaveChangesAsync(cancellationToken);
        })
        .WithTags("Feeds");

        // delete a specific user-submitted feed
        app.MapDelete("v1/feeds/{id}", async (PodcastDbContext podcastDbContext, Guid id, CancellationToken cancellationToken) =>
        {
            var feed = podcastDbContext.UserSubmittedFeeds.FirstOrDefault(x => x.Id == id);
            if (feed is null)
                return Results.NotFound();

            podcastDbContext.Remove(feed);
            await podcastDbContext.SaveChangesAsync(cancellationToken);
            return Results.Accepted();
        })
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status202Accepted)
        .WithName("DeleteFeed")
        .WithTags("Feeds");
    }
}
