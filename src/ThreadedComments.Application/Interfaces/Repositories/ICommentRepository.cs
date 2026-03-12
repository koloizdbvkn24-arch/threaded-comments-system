using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Interface.Repositories;


public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken ct);
}