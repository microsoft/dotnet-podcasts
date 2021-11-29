namespace ListenTogether.Domain;

public class Show
{
    protected Show() {}

    public Show(Guid id, string title, string author, string image)
    {
        Id = Guid.NewGuid();
        ShowId = id;
        Title = title;
        Author = author;
        Image = image;
    }

    public Guid Id { get; private set; }
    public Guid ShowId { get; private  set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Image { get; private set; }
}