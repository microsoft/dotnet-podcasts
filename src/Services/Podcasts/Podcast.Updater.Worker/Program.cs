using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Podcast.Updater.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configuration => {
        var credential = new ChainedTokenCredential(new AzureDeveloperCliCredential(), new DefaultAzureCredential());
        configuration.AddAzureKeyVault(new Uri(Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_ENDPOINT")!), credential);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services
            .AddDbContext<PodcastDbContext>(options =>
            {
                options.UseSqlServer(
                    hostContext.Configuration[hostContext.Configuration["AZURE_API_SQL_CONNECTION_STRING_KEY"]],
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