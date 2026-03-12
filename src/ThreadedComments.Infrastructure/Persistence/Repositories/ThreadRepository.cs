using Microsoft.EntityFrameworkCore;
using ThreadedComments.Application.Interface.Repositories;
using DomainThread = ThreadedComments.Domain.Entities.Thread;

namespace ThreadedComments.Infrastructure.Persistence.Repositories;


public sealed class ThreadRepository : IThreadRepository
{ 
    private readonly AppDbContext _context;

    public ThreadRepository(AppDbContext context){
        _context = context;
    }

    public async Task<DomainThread?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Threads
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}