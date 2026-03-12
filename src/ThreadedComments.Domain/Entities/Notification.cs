using ThreadedComments.Domain.Enums;

namespace ThreadedComments.Domain.Entities;


public sealed class Notification
{
    public Guid Id { get; private set; }
    public Guid RecipientAuthorId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public bool IsRead { get; private set; }

    private Notification(){}

    public Notification(
        Guid id,
        Guid recipientAuthorId,
        NotificationType type,
        string message,
        DateTime createdAt
    )
    {
        if(id == Guid.Empty)
            throw new ArgumentException("Notification id cannot be empty", nameof(id));

        if(recipientAuthorId == Guid.Empty)
            throw new ArgumentException("Author id cannot be empty", nameof(recipientAuthorId));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Notification message is not empty", nameof(message));

        Id = id;
        RecipientAuthorId = recipientAuthorId;
        Type = type;
        Message = message;
        CreatedAt = CreatedAt;
    }

    public void MarkAsRead()
    {
        IsRead = true;  
    }
}