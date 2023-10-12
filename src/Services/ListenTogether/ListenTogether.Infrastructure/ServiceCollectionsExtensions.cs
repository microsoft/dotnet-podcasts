using Azure.Identity;
using ListenTogether.Application.Interfaces;
using ListenTogether.Infrastructure.Data;
using ListenTogether.Infrastructure.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ListenTogether.Infrastructure
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration[configuration["AZURE_HUB_SQL_CONNECTION_STRING_KEY"]];
            serviceCollection.AddSqlServer<ListenTogetherDbContext>(connectionString);
            serviceCollection.AddScoped<IApplicationDbContext, ListenTogetherDbContext>();
            serviceCollection.AddHttpClient<IEpisodesClient, EpisodesHttpClient>(opt =>
            {
                opt.BaseAddress = new Uri(configuration["REACT_APP_API_BASE_URL"]);
                opt.DefaultRequestHeaders.Add("api-version", "1.0");
            });

            return serviceCollection;
        }
    }
}
