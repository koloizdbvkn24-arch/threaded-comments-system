using ThreadedComments.Application.DTOs.Comments;

namespace ThreadedComments.Application.Interface.Services;


public interface ICommentService
{
    Task<CommentDto> AddRootAsync(Guid threadId, AddRootCommentsRequest request, CancellationToken ct);
}