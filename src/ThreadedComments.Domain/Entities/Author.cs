namespace ThreadedComments.Domain.Entities;


public sealed class Author
{
    public Guid Id { get; private set; }
    public string DisplayName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } 

    private Author(){}

    public Author(Guid id, string displayName, DateTime createdAt)
    {
        if(id == Guid.Empty)
            throw new ArgumentException("Author id cannot be empty", nameof(id));

        if(string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty", nameof(displayName));

        Id = id;
        DisplayName = displayName;
        CreatedAt = createdAt;
    }
}