using Podcast.Updater.Worker;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddDbContext<PodcastDbContext>(options =>
            {
                options.UseSqlServer(
                    hostContext.Configuration.GetConnectionString("PodcastDb"),
                    builder =>
                    {
                        builder.EnableRetryOnFailure(10, TimeSpan.FromSeconds(60), null);
                        builder.CommandTimeout(30);
                    }
                );
            })
            .AddTransient<IPodcastUpdateHandler, PodcastUpdateHandler>()
            .AddHttpClient<IFeedClient, FeedClient>()
            .Services
            .AddLogging();
    })
    .Build();

await UpdatePodcasts(host.Services);

await host.RunAsync();

static async Task UpdatePodcasts(IServiceProvider hostProvider)
{
    using var scope = hostProvider.CreateScope();
    var handler = scope.ServiceProvider.GetRequiredService<IPodcastUpdateHandler>();
    await handler.HandleUpdateAsync();
}