namespace ThreadedComments.Application.DTOs.Comments;


public sealed class CommentTreeItemDto
{
    public Guid Id { get; init; }
    public Guid ThreadId { get; init; }
    public Guid AuthorId { get; init; }
    public Guid? ParentId { get; init; }
    public string Text { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdateAt { get; init; }

    public List<CommentTreeItemDto> Replies { get; init; } = new();
}