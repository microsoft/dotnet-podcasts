using Microsoft.AspNetCore.SignalR.Client;

namespace Podcast.Components;

public enum PlayerState { Paused, Playing };
public record RoomShow(Guid Id, string Title, string Author, string Image);
public record RoomEpisode(Guid Id, string Title, string Description, string Url, DateTime Published, TimeSpan? Duration, RoomShow Show);
public record User(string Name);
public record Room(TimeSpan Progress, PlayerState PlayerState, string Code, RoomEpisode Episode, List<User> Users);

public class ListenTogetherHubClient : IAsyncDisposable
{
    private string _hubUrl;
    private bool _initialized = false;
    private HubConnection? _hubConnection;

    public event Action<string, string>? MessageReceived;
    public event Action<Room>? PlayerStateUpdated;
    public event Action<Room>? RoomOpened;
    public event Action<Room>? RoomUpdated;

    public ListenTogetherHubClient(string hubUrl)
    {
        _hubUrl = hubUrl;
    }

    public async Task Initialize()
    {
        if (!_initialized)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
                MessageReceived?.Invoke(user, message));

            _hubConnection.On<Room>("UpdatePlayerState", (room) =>
                PlayerStateUpdated?.Invoke(room));

            _hubConnection.On<Room>("RoomOpened", (room) =>
                RoomOpened?.Invoke(room));

            _hubConnection.On<Room>("UpdateRoom", (room) =>
                RoomUpdated?.Invoke(room));

            await _hubConnection.StartAsync();

            _initialized = true;
        }
    }

    public Task OpenRoom(string user, Guid episodeId) =>
        _hubConnection!.SendAsync(nameof(OpenRoom), user, episodeId);

    public Task JoinRoom(string user, string room) =>
       _hubConnection!.InvokeAsync(nameof(JoinRoom), user, room);

    public Task LeaveRoom(string room) =>
       _hubConnection!.SendAsync(nameof(LeaveRoom), room);

    public Task SendMessage(string user, string message, string room) =>
       _hubConnection!.SendAsync(nameof(SendMessage), user, message, room);

    public Task UpdatePlayerState(long progress, bool isPlaying, string room) =>
       _hubConnection!.SendAsync(nameof(UpdatePlayerState), progress, isPlaying ? PlayerState.Playing : PlayerState.Paused, room);

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
            _initialized = false;
        }
    }
}