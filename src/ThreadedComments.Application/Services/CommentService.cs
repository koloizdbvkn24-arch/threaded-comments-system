using ThreadedComments.Application.Interface.Factories;
using ThreadedComments.Application.DTOs.Comments;
using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.Common.Exceptions;

namespace ThreadedComments.Application.Services;


public sealed class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IThreadRepository _threadRepository;
    private readonly ICommentFactory _commentFactory;
    private readonly IAuthorRepository _authorRepository;

    public CommentService(
        ICommentRepository commentRepository,
        IThreadRepository threadRepository,
        ICommentFactory componentFactory,
        IAuthorRepository authorRepository
    )
    {
        _commentRepository = commentRepository;
        _threadRepository = threadRepository;
        _commentFactory = componentFactory;
        _authorRepository = authorRepository;
    }

    public async Task<CommentDto> AddRootAsync(Guid threadId, CreateCommentRequest request, CancellationToken ct)
    {
        if(threadId == Guid.Empty)
            throw new ArgumentException("Thread id cannot be empty.", nameof(threadId));
        
        if(request is null)
            throw new ArgumentException(nameof(request));

        var thread = await _threadRepository.GetByIdAsync(threadId, ct);

        if(thread is null)
            throw new NotFoundException("Thread was not found.");

        var author = await _authorRepository.GetByIdAsync(request.AuthorId, ct);

        if(author is null)
            throw new NotFoundException("Author was not found.");

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

    public async Task<CommentDto> AddReplyAsync(
        Guid threadId,
        Guid parentCommentId,
        CreateCommentRequest request,
        CancellationToken ct
    )
    {
        if(threadId == Guid.Empty)
            throw new ArgumentException("Thread id cannot be empty.", nameof(threadId));

        if(parentCommentId == Guid.Empty)
            throw new ArgumentException("Parent comment id cannot be empty.", nameof(parentCommentId));

        if(request is null)
            throw new ArgumentNullException(nameof(request));

        var thread = await _threadRepository.GetByIdAsync(threadId, ct);
        if(thread is null)
            throw new NotFoundException("Thread was not found.");

        var author = await _authorRepository.GetByIdAsync(request.AuthorId, ct);
        if(author is null)
            throw new NotFoundException("Author was not found.");

        var parentComment = await _commentRepository.GetByIdAsync(parentCommentId, ct);
        if(parentComment is null)
            throw new NotFoundException("Parent comment was not found.");

        if(parentComment.ThreadId != threadId)
            throw new ValidationException("Parent comment not belond to the specified thread.");

        var reply = _commentFactory.CreateReply(
            threadId,
            request.AuthorId,
            parentCommentId,
            request.Text
        );

        await _commentRepository.AddAsync(reply, ct);

        return MapToDto(reply);
    }

    private static CommentDto MapToDto(ThreadedComments.Domain.Entities.Comment comment)
    {
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