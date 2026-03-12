namespace ThreadedComments.Application.DTOs.Authors;


public sealed record AuthorDto(
    Guid Id,
    string DisplayName,
    DateTime CreatedAt
);