var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<PodcastDbContext>(options =>
        {
            options.UseSqlServer(
                hostContext.Configuration.GetConnectionString("PodcastDb"),
                builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    builder.CommandTimeout(10);
                }
            );
        });
        var feedQueueClient = new QueueClient(hostContext.Configuration.GetConnectionString("FeedQueue"), "feed-queue");
        feedQueueClient.CreateIfNotExists();
        services.AddSingleton(feedQueueClient);
        services.AddScoped<IPodcastIngestionHandler, PodcastIngestionHandler>();
        services.AddHttpClient<IFeedClient, FeedClient>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();