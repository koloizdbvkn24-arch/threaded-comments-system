namespace ThreadedComments.Domain.Entities;


public sealed class Thread
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public DateTime CreadetAt { get; private set; }

    private Thread()
    {
        
    }

    public Thread(Guid id, string title, DateTime creadetAt)
    {
        if(id == Guid.Empty)
            throw new ArgumentException("Thread is not be empty");

        if(string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Thread title cannot be empty");

        Id = id;
        Title = title;
        CreadetAt = creadetAt;
    }
}