using ThreadedComments.Application.DTOs.Comments;
using ThreadedComments.Application.DTOs.Reactions;

namespace ThreadedComments.Application.Interface.Services;


public interface IReactionService
{
    Task SetReactionAsync(Guid commentId, SetReactionRequest request, CancellationToken ct);
    Task RemoveReactionAsync(Guid commentId, RemoveReactionRequest request, CancellationToken ct);
}