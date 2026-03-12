using ThreadedComments.Application.Interface.Factories;
using ThreadedComments.Application.DTOs.Comments;
using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Application.Interface.Services;

namespace ThreadedComments.Application.Services;


public sealed class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IThreadRepository _threadRepository;
    private readonly ICommentFactory _commentFactory;

    public CommentService(
        ICommentRepository commentRepository,
        IThreadRepository threadRepository,
        ICommentFactory componentFactory
    )
    {
        _commentRepository = commentRepository;
        _threadRepository = threadRepository;
        _commentFactory = componentFactory;
    }

    public async Task<CommentDto> AddRootAsync(Guid threadId, AddRootCommentsRequest request, CancellationToken ct)
    {
        if(threadId == Guid.Empty)
            throw new ArgumentException("Thread id cannot be empty.", nameof(threadId));
        
        if(request is null)
            throw new ArgumentException(nameof(request));

        var thread = _threadRepository.GetByIdAsync(threadId, ct);

        if(thread is null)
        {
            throw new InvalidOperationException("Thrad was not found.");
        }

        var comment = _commentFactory.CreateRoot(threadId, request.AuthorId, request.Text);

        await _commentRepository.AddAsync(comment, ct);

        return new CommentDto
        {
            Id = comment.Id,
            ThreadId = comment.ThreadId,
            AuthorId = comment.AuthorId,
            ParentId = comment.ParentId,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt,
            UpdateAt = comment.UpdateAt
        };
    }
}