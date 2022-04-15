using ListenTogether.Domain;
using Orleans;

namespace ListenTogether.Application.Interfaces;

public interface IRoomGrain : IGrainWithStringKey
{
    Task SetRoom(Room room);
    Task<Room> JoinRoom(string connectionId, string userName);
    Task<Room> LeaveRoom(string connectionId);
    Task<Room> UpdatePlayerState(TimeSpan seconds, PlayerState playerState);
}
