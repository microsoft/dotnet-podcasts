using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Podcast.API.Models;
using Podcast.Infrastructure.Data;
using System.Threading;

namespace Podcast.API.Routes;

public static class BlockingApi
{
    public static RouteGroupBuilder MapBlockingApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", BlockingTask);
        return group;
    }

    public static Ok<string> BlockingTask()
    {
        Task.Delay(TimeSpan.FromMinutes(10)).Wait();
        return TypedResults.Ok("succeeded");
    }
}
