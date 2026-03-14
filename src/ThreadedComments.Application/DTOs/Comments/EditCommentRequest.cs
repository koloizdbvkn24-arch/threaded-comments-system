namespace ThreadedComments.Application.DTOs.Comments;

public sealed record EditCommentRequest(
    Guid AuthorId,
    string NewText
);