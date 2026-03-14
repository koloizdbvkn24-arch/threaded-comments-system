using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Interface.Repositories;


public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken ct);
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Comment>> GetByThreadIdAsync(Guid threadId, CancellationToken ct);
    Task UpdateAsync(Comment comment, CancellationToken ct);
    Task DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken ct);
}