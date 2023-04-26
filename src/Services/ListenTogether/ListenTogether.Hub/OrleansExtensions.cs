using Azure.Identity;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace ListenTogether.Hub
{
    public static class OrleansExtensions
    {
        public static WebApplicationBuilder AddOrleans(this WebApplicationBuilder builder)
        {
            var credential = new ChainedTokenCredential(new AzureDeveloperCliCredential(), new DefaultAzureCredential());
            builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"]), credential);
            builder.Host.UseOrleans(silo =>
             {
                var connectionString = builder.Configuration[builder.Configuration["AZURE_ORLEANS_STORAGE_CONNECTION_STRING_KEY"]];
                 silo
                    .Configure<ClusterOptions>(options =>
                     {
                         options.ClusterId = "NetPodcastCluster";
                         options.ServiceId = "NetPodcastService";
                     })
                    .UseAzureStorageClustering(options => options.ConfigureTableServiceClient(connectionString))
                    .AddAzureTableGrainStorage("roomStorage", op => op.ConfigureTableServiceClient(connectionString))
                    .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                    .UseDashboard(options =>
                     {
                         options.BasePath = "/dashboard";
                         options.HideTrace = true;
                         options.HostSelf = false;
                     });
             });

            return builder;
        }

        public static WebApplication MapOrleansDashboard(this WebApplication app)
        {
            app.Map("/dashboard", d =>
            {
                d.UseOrleansDashboard();
            });

            return app;
        }
    }
}
