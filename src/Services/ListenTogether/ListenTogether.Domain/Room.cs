namespace ListenTogether.Domain;

public class Room
{
    private const int Length = 5;
    private const string ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly Random _randGenerator = Random.Shared;

    public Room () 
    {
        Code = GenerateRoomCode();
    }

    public Room(Episode episode)
    {
        UpdatedAt = DateTime.UtcNow;
        ProgressAt = TimeSpan.Zero;

        Code = GenerateRoomCode();
        PlayerState = PlayerState.Paused;
        Episode = episode;
    }

    public string Code { get; private set; }
    public PlayerState PlayerState { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public TimeSpan ProgressAt { get; private set; }
    public Episode Episode { get; private set; }
    private TimeSpan ElapsedTime => DateTime.UtcNow - UpdatedAt;
    public TimeSpan Progress => PlayerState == PlayerState.Paused ? ProgressAt : ProgressAt + ElapsedTime;
    public ICollection<User> Users { get; private set; } = new List<User>();


    public void UpdatePlayerState(TimeSpan progress, PlayerState state)
    {
        ProgressAt = progress;
        UpdatedAt = DateTime.UtcNow;
        PlayerState = state;
    }

    private string GenerateRoomCode()
    {
        return new string(Enumerable.Repeat(ValidCharacters, Length)
            .Select(s => s[_randGenerator.Next(s.Length)])
            .ToArray());
    }

    public void RemoveUser(string connectionId)
    {
        var user = Users.FirstOrDefault(u => u.ConnectionId == connectionId);
        if (user != null)
        {
            Users.Remove(user);
        }
    }

    public bool IsEmpty()
    {
        return Users.Count == 0;
    }

    public void AddUser(User user)
    {
        Users.Add(user);
    }
}