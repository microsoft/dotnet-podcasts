namespace ListenTogether.Application;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<GetUserRoomQueryRequest, Room?>, GetUserRoomQueryHandler>();
        services.AddScoped<IRequestHandler<GetRoomsRequest, IList<Room>>, GetRoomsQueryQueryHandler>();

        services.AddScoped<IRequestHandler<JoinRoomRequest, Room>, JoinRoomRequestHandler>();
        services.AddScoped<IRequestHandler<LeaveRoomRequest, Room>, LeaveRoomRequestHandler>();
        services.AddScoped<IRequestHandler<OpenRoomRequest, Room>, OpenRoomRequestHandler>();
        services.AddScoped<IRequestHandler<UpdatePlayerRequest, Room>, UpdatePlayerRequestHandler>();
        
        return services;
    }
}