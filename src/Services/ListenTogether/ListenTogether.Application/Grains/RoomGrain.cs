using ListenTogether.Application.Interfaces;
using ListenTogether.Domain;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;

namespace ListenTogether.Application.Grains
{
    public sealed class RoomGrain : Grain, IRoomGrain
    {
        private readonly IPersistentState<Room> _room;
        private readonly ILogger<RoomGrain> _logger;

        public RoomGrain([PersistentState("room", "roomStorage")] IPersistentState<Room> room,
            ILogger<RoomGrain> logger)
        {
            _room = room;
            _logger = logger;
        }

        public async Task<Room> JoinRoom(string connectionId, string userName)
        {
            _logger.LogInformation($"User {userName} is joining {_room.State.Code}, listening to {_room.State.Episode.Show.Title}");
            _room.State.AddUser(new User(connectionId, userName));
            await _room.WriteStateAsync();
            return _room.State;
        }

        public async Task<Room> LeaveRoom(string connectionId)
        {
            var user = _room.State.Users.First(_ => _.ConnectionId == connectionId);
            _logger.LogInformation($"User {user.Name} is joining {_room.State.Code}, listening to {_room.State.Episode.Show.Title}");
            _room.State.RemoveUser(connectionId);
            await _room.WriteStateAsync();

            if (_room.State.Users.Count is 0)
            {
                base.DeactivateOnIdle();
            }

            return _room.State;
        }

        public async Task SetRoom(Room room)
        {
            _room.State = room;
            await _room.WriteStateAsync();
        }

        public Task<Room> UpdatePlayerState(TimeSpan seconds, PlayerState playerState)
        {
            _logger.LogInformation($"Updating player state to {playerState} at {seconds}.");
            _room.State.UpdatePlayerState(seconds, playerState);
            return Task.FromResult(_room.State);
        }
    }
}
