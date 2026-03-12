using ThreadedComments.Application.DTOs.Authors;

namespace ThreadedComments.Application.Interface.Services;


public interface IAuthorService
{
    Task<AuthorDto> CreateAsync(CreateAuthorRequest request, CancellationToken ct);
}