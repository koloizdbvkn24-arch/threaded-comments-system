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
        await _context.Comments.AddAsync(comment, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Comment>> GetByThreadIdAsync(Guid threadId, CancellationToken ct)
    {
        return await _context.Comments
            .AsNoTracking()
            .Where(x => x.ThreadId == threadId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task UpdateAsync(Comment comment, CancellationToken ct)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken ct)
    {
        var comments = await _context.Comments
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(ct);

        _context.Comments.RemoveRange(comments);
        await _context.SaveChangesAsync(ct);
    }
}