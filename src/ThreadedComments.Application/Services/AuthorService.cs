using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.DTOs.Authors;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Services;


public sealed class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<AuthorDto> CreateAsync(CreateAuthorRequest request, CancellationToken ct)
    {
        if(request is null)
            throw new ArgumentNullException(nameof(request));

        var author = new Author(
            Guid.NewGuid(),
            request.DisplayName,
            DateTime.UtcNow
        );

        await _authorRepository.AddAsync(author, ct);

        return new AuthorDto(
            author.Id,
            author.DisplayName,
            author.CreatedAt
        );
    }
}