using Azure.Identity;
using Microsoft.Extensions.Configuration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configuration => {
        var credential = new ChainedTokenCredential(new AzureDeveloperCliCredential(), new DefaultAzureCredential());
        configuration.AddAzureKeyVault(new Uri(Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_ENDPOINT")!), credential);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<PodcastDbContext>(options =>
        {
            options.UseSqlServer(
                hostContext.Configuration[hostContext.Configuration["AZURE_API_SQL_CONNECTION_STRING_KEY"]],
                builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    builder.CommandTimeout(10);
                }
            );
        });
        var feedQueueClient = new QueueClient(hostContext.Configuration[hostContext.Configuration["AZURE_FEED_QUEUE_CONNECTION_STRING_KEY"]], "feed-queue");
        feedQueueClient.CreateIfNotExists();
        services.AddSingleton(feedQueueClient);
        services.AddScoped<IPodcastIngestionHandler, PodcastIngestionHandler>();
        services.AddHttpClient<IFeedClient, FeedClient>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();