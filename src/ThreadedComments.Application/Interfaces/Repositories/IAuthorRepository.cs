using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Interface.Repositories;


public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Author author, CancellationToken ct);
}