using Microsoft.EntityFrameworkCore;
using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Persistence.Repositories;


public sealed class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Author?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(Author author, CancellationToken ct)
    {
        await _context.Authors.AddAsync(author, ct);
        await _context.SaveChangesAsync(ct);
    }
}