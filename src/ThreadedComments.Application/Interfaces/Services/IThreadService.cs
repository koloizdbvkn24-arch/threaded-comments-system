using ThreadedComments.Application.DTOs.Treads;

namespace ThreadedComments.Application.Interface.Services;

public interface IThreadService
{
    Task<ThreadDto> CreateAsync(CreateThreadRequest request, CancellationToken ct);
}