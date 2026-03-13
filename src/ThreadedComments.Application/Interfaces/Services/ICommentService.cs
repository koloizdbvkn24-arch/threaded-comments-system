using ThreadedComments.Application.DTOs.Comments;

namespace ThreadedComments.Application.Interface.Services;


public interface ICommentService
{
    Task<CommentDto> AddRootAsync(Guid threadId, CreateCommentRequest request, CancellationToken ct);

    Task<CommentDto> AddReplyAsync(
        Guid threadId,
        Guid parentCommentId,
        CreateCommentRequest request,
        CancellationToken ct
    );

    Task<IReadOnlyList<CommentTreeItemDto>> GetThreadCommentsAsync(Guid threadId, CancellationToken ct);
}