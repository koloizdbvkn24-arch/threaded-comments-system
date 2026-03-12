using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.DTOs.Treads;
using ThreadedComments.Application.Interface.Repositories;
using DomainThread = ThreadedComments.Domain.Entities.Thread;

namespace ThreadedComments.Application.Services;


public sealed class ThreadService : IThreadService
{
    private readonly IThreadRepository _threadRepository;
    private readonly IAuthorRepository _authorRepository;
    
    public ThreadService(
        IThreadRepository threadRepository,
        IAuthorRepository authorRepository
        )
    {
        _threadRepository = threadRepository;
        _authorRepository = authorRepository;
    }

    public async Task<ThreadDto> CreateAsync(CreateThreadRequest request, CancellationToken ct)
    {
        if(request is null)
            throw new ArgumentNullException(nameof(request));

        var author = await _authorRepository.GetByIdAsync(request.AuthorId, ct);

        if(author is null)
            throw new InvalidOperationException("Author id not found");

        var thread = new DomainThread(
            Guid.NewGuid(),
            request.AuthorId,
            request.Title,
            DateTime.UtcNow
        );

        await _threadRepository.AddAsync(thread, ct);

        return new ThreadDto(
            thread.Id,
            thread.AuthorId,
            thread.Title,
            thread.CreadetAt
        );
    }
}