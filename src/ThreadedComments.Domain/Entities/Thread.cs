namespace ThreadedComments.Domain.Entities;


public sealed class Thread
{
    public Guid Id { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public DateTime CreadetAt { get; private set; }

    private Thread()
    {
        
    }

    public Thread(Guid id, Guid authorId, string title, DateTime creadetAt)
    {
        if(id == Guid.Empty)
            throw new ArgumentException("Thread id cannot be empty");

        if(authorId == Guid.Empty)
            throw new ArgumentException("Author id cannot be empty");

        if(string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Thread title cannot be empty");

        Id = id;
        AuthorId = authorId;
        Title = title;
        CreadetAt = creadetAt;
    }
}