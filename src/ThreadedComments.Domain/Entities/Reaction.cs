using ThreadedComments.Domain.Enums;

namespace ThreadedComments.Domain.Entities;


public sealed class Reaction
{
    public Guid Id { get; private set; }
    public Guid CommentId { get; private set; }
    public Guid AuthorId { get; private set; }
    public ReactionType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Reaction(){}

    public Reaction(
        Guid id,
        Guid commentId,
        Guid authorId,
        ReactionType type,
        DateTime createdAt
    )
    {
        if(id == Guid.Empty)
            throw new ArgumentException("Reaction id cannot be empty", nameof(id));

        if(commentId == Guid.Empty)
            throw new ArgumentException("Comment id cannot be empty", nameof(commentId));

        if(authorId == Guid.Empty)
            throw new ArgumentException("Author id cannot be empty", nameof(authorId));

        Id = id;
        CommentId = commentId;
        AuthorId = authorId;
        Type = type;
        CreatedAt = createdAt;
    }

    public void ChangeType(ReactionType newType)
    {
        Type = newType;
    }
}