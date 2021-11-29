namespace ListenTogether.Domain;

public class User
{
    protected User() { }

    public User(string connectionId, string name)
    {
        ConnectionId = connectionId;
        Name = name;
    }

    public Guid Id { get; private set; }
    public string ConnectionId { get; private set; }
    public string Name { get; private set; }
}