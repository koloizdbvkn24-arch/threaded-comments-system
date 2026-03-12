public record CommentDto
{
    public Guid Id { get; init; }
    public Guid AuthorId { get; init; }
    public Guid ThreadId { get; init; }
    public Guid? ParentId { get; init; }
    public string Text { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdateAt { get; init; } 
}