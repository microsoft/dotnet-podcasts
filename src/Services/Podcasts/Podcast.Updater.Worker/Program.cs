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

await host.RunAsync();