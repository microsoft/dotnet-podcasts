using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace ListenTogether.Application.Grains
{
    public class RoomGrain : Grain, IRoomGrain
    {
        IPersistentState<Room> Room { get; set; }
        public ILogger<RoomGrain> Logger { get; }

        public RoomGrain([PersistentState("room", "roomStorage")] IPersistentState<Room> room,
            ILogger<RoomGrain> logger)
        {
            Room = room;
            Logger = logger;
        }

        public async Task<Room> JoinRoom(string connectionId, string userName)
        {
            Logger.LogInformation($"User {userName} is joining {Room.State.Code}, listening to {Room.State.Episode.Show.Title}");
            Room.State.AddUser(new User(connectionId, userName));
            await Room.WriteStateAsync();
            return Room.State;
        }

        public async Task<Room> LeaveRoom(string connectionId)
        {
            var user = Room.State.Users.First(x => x.ConnectionId == connectionId);
            Logger.LogInformation($"User {user.Name} is joining {Room.State.Code}, listening to {Room.State.Episode.Show.Title}");
            Room.State.RemoveUser(connectionId);
            await Room.WriteStateAsync();

            if(Room.State.Users.Count == 0)
            {
                base.DeactivateOnIdle();
            }

            return Room.State;
        }

        public async Task SetRoom(Room room)
        {
            Room.State = room;
            await Room.WriteStateAsync();
        }

        public Task<Room> UpdatePlayerState(TimeSpan seconds, PlayerState playerState)
        {
            Logger.LogInformation($"Updating player state to {playerState} at {seconds}.");
            Room.State.UpdatePlayerState(seconds, playerState);
            return Task.FromResult(Room.State);
        }
    }
}
