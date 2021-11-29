using ListenTogether.Application.Interfaces;
using ListenTogether.Application.Rooms;
using ListenTogether.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ListenTogether.Application;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        AddQueries(serviceCollection);
        AddCommands(serviceCollection);
        return serviceCollection;
    }

    private static void AddCommands(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRequestHandler<JoinRoomRequest, Room>, JoinRoomRequestHandler>();
        serviceCollection.AddScoped<IRequestHandler<LeaveRoomRequest, Room>, LeaveRoomRequestHandler>();
        serviceCollection.AddScoped<IRequestHandler<OpenRoomRequest, Room>, OpenRoomRequestHandler>();
        serviceCollection.AddScoped<IRequestHandler<UpdatePlayerRequest, Room>, UpdatePlayerRequestHandler>();
    }

    private static void AddQueries(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRequestHandler<GetUserRoomQuery, Room?>, GetUserRoomQueryHandler>();
        serviceCollection.AddScoped<IRequestHandler<GetRoomsRequest, IList<Room>>, GetRoomsQueryQueryHandler>();
    }
}