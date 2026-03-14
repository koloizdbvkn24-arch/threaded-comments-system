namespace ThreadedComments.Application.DTOs.Comments;

public sealed record DeleteCommentBranchRequest(
    Guid AuthorId
);
