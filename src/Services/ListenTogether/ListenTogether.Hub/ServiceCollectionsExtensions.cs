namespace ListenTogether.Hub;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddHub(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(setup =>
        {
            setup.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowed(_ => true).AllowCredentials().AllowAnyHeader().AllowAnyMethod());
        });
        serviceCollection.AddSignalR();

        return serviceCollection;
    }
}