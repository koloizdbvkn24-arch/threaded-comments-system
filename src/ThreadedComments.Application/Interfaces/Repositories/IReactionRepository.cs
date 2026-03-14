using ThreadedComments.Domain.Entities;
using DomainThread = ThreadedComments.Domain.Entities.Thread;

namespace ThreadedComments.Application.Interface.Repositories;


public interface IReactionRepository
{
    Task<Reaction?> GetByCommentAndAuthorAsync(Guid commentId, Guid authorId, CancellationToken ct);
    Task<List<Reaction>> GetByCommentIdAsync(IEnumerable<Guid> commentIds, CancellationToken ct);
    Task AddAsync(Reaction reaction, CancellationToken ct);
    Task UpdateAsync(Reaction reaction, CancellationToken ct);
    Task DeleteAsync(Reaction reaction, CancellationToken ct);
}