namespace ThreadedComments.Application.DTOs.Reactions;


public sealed record RemoveReactionRequest(
    Guid AuthorId
);