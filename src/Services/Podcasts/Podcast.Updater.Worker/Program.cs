using Podcast.Updater.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services
            .AddDbContext<PodcastDbContext>(options =>
            {
                options.UseSqlServer(
                    hostContext.Configuration.GetConnectionString("PodcastDb"),
                    builder =>
                    {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                        builder.CommandTimeout(10);
                    }
                );
            })
            .AddTransient<IPodcastUpdateHandler, PodcastUpdateHandler>()
            .AddHttpClient<IFeedClient, FeedClient>()
            .Services
            .AddLogging();
    })
    .Build();

await host.RunAsync();