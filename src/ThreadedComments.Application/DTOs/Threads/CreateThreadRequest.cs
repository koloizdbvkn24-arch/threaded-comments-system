namespace ThreadedComments.Application.DTOs.Treads;

public sealed record CreateThreadRequest(
    Guid AuthorId,
    string Title
);