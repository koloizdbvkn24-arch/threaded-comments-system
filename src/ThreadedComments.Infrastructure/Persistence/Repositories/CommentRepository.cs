using Microsoft.EntityFrameworkCore;
using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Persistence.Repositories;


public sealed class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment, CancellationToken ct)
    {
        await _context.AddAsync(comment, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}