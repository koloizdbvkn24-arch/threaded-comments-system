namespace ThreadedComments.Application.DTOs.Treads;


public sealed record ThreadDto(
    Guid Id,
    Guid AuthorId,
    string Title,
    DateTime CreatedAt
);