using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Persistence.Repositories;


public sealed class ReactionRepository : IReactionRepository
{
    private readonly AppDbContext _context;

    public ReactionRepository(
        AppDbContext contetx
    )
    {
        _context = contetx;
    }

    public async Task<Reaction?> GetByCommentAndAuthorAsync(Guid commentId, Guid authorId, CancellationToken ct)
    {
        return await _context.Reactions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.CommentId == commentId && x.AuthorId == authorId
            );
    }

    public async Task<List<Reaction>> GetByCommentIdAsync(IEnumerable<Guid> commentIds, CancellationToken ct)
    {
        return await _context.Reactions
            .AsNoTracking()
            .Where(x => commentIds.Contains(x.CommentId))
            .ToListAsync(ct);
    }

    public async Task AddAsync(Reaction reaction, CancellationToken ct)
    {
        await _context.Reactions.AddAsync(reaction, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Reaction reaction, CancellationToken ct)
    {
        _context.Reactions.Update(reaction);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Reaction reaction, CancellationToken ct)
    {
        _context.Reactions.Remove(reaction);
        await _context.SaveChangesAsync(ct);
    }
}