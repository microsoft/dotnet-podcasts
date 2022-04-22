using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace ListenTogether.Hub
{
    public static class OrleansExtensions
    {
        public static WebApplicationBuilder AddOrleans(this WebApplicationBuilder builder)
        {
            builder.Host.UseOrleans(silo =>
             {
                 var connectionString = builder.Configuration.GetConnectionString("OrleansStorage");
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
