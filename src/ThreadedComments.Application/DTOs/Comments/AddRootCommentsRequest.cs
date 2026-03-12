namespace ThreadedComments.Application.DTOs.Comments;


public sealed record AddRootCommentsRequest(
    Guid AuthorId,
    string Text
);