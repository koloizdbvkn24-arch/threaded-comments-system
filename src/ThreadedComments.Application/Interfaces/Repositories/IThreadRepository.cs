using DomainThread = ThreadedComments.Domain.Entities.Thread;

namespace ThreadedComments.Application.Interface.Repositories;


public interface IThreadRepository
{
    Task<DomainThread?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(DomainThread thread, CancellationToken ct);
}