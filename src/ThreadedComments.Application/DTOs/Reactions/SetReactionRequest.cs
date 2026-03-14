using ThreadedComments.Domain.Enums;

namespace ThreadedComments.Application.DTOs.Reactions;


public sealed record SetReactionRequest(
    Guid AuthorId,
    ReactionType Type
);