using ThreadedComments.Application.Interface.Factories;
using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.Interface.Traversal;
using ThreadedComments.Application.Common.Exceptions;
using ThreadedComments.Application.DTOs.Comments;
using ThreadedComments.Domain.Entities;


namespace ThreadedComments.Application.Services;


public sealed class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IThreadRepository _threadRepository;
    private readonly ICommentFactory _commentFactory;
    private readonly IAuthorRepository _authorRepository;
    private readonly ICommentTraversal _commentTraversal;
    private readonly IReactionRepository _reactionRepository;

    public CommentService(
        ICommentRepository commentRepository,
        IThreadRepository threadRepository,
        ICommentFactory componentFactory,
        IAuthorRepository authorRepository,
        ICommentTraversal commentTraversal,
        IReactionRepository reactionRepository
    )
    {
        _commentRepository = commentRepository;
        _threadRepository = threadRepository;
        _commentFactory = componentFactory;
        _authorRepository = authorRepository;
        _commentTraversal = commentTraversal;
        _reactionRepository = reactionRepository;
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
            UpdatedAt = comment.UpdateAt
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

    public async Task<IReadOnlyList<CommentTreeItemDto>> GetThreadCommentsAsync(Guid threadId, CancellationToken ct)
    {
        if(threadId == Guid.Empty)
            throw new ArgumentException("Thread id cannot be empty.", nameof(threadId));

        var thread = await _threadRepository.GetByIdAsync(threadId, ct);
        if(thread is null)
            throw new NotFoundException("Thread was not found.");

        var comments = await _commentRepository.GetByThreadIdAsync(threadId, ct);

        if(comments.Count == 0)
            return [];

        var commentIds = comments.Select(x => x.Id).ToList();
        var reactions = await _reactionRepository.GetByCommentIdAsync(commentIds, ct);

        var reactionStats = BuildReactionStats(reactions);
        
        return BuildTree(comments, reactionStats);
    }

    public async Task<CommentDto> EditCommentAsync(Guid commentId, EditCommentRequest request, CancellationToken ct)
    {
        if(commentId == Guid.Empty)
            throw new ArgumentException("Comment id cannot be empty.", nameof(commentId));

        if(request is null)
            throw new ArgumentNullException(nameof(request));

        var comment = await _commentRepository.GetByIdAsync(commentId, ct);
        if(comment is null)
            throw new NotFoundException("Comment was not found.");

        var author = await _authorRepository.GetByIdAsync(request.AuthorId, ct);
        if(author is null)
            throw new NotFoundException("Author was not found.");

        if(comment.AuthorId != request.AuthorId)
            throw new ForbiddenException("You can edit only your own comments.");

        comment.Edit(request.NewText, DateTime.UtcNow);

        await _commentRepository.UpdateAsync(comment, ct);

        return MapToDto(comment);
    }

    public async Task DeleteCommentAsync(Guid commentId, DeleteCommentBranchRequest request, CancellationToken ct)
    {
        if(commentId == Guid.Empty)
            throw new ArgumentException("Comment id cannot be empty.", nameof(commentId));

        if(request is null)
            throw new ArgumentNullException(nameof(request));

        var comment = await _commentRepository.GetByIdAsync(commentId, ct);
        if(comment is null)
            throw new NotFoundException("Comment was not found.");

        var author = await _authorRepository.GetByIdAsync(request.AuthorId, ct);
        if(author is null)
            throw new NotFoundException("Author was not found.");

        if(comment.AuthorId != request.AuthorId)
            throw new ForbiddenException("You can delete ony own comment branch.");

        var commentOfThread = await _commentRepository.GetByThreadIdAsync(comment.ThreadId, ct);

        var commentById = BuildComment(commentOfThread);

        if(!commentById.TryGetValue(commentId, out var rootOfBranch))
            throw new NotFoundException("Comment branch root was not found.");

        var idsToDelete = CollectBranchIds(rootOfBranch);

        await _commentRepository.DeleteRangeAsync(idsToDelete, ct);
    }

    // Допоміжні методи
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
            UpdatedAt = comment.UpdateAt
        };
    }

    private static IReadOnlyList<CommentTreeItemDto> BuildTree(
        List<Comment> comments,
        Dictionary<Guid, (int likes, int dislikes, int score)> reactionStats
        )
    {
        var nodes = comments.ToDictionary(
            comment => comment.Id,
            comment =>
        {
            reactionStats.TryGetValue(comment.Id, out var stats);

            return new CommentTreeItemDto
            {
                Id = comment.Id,
                ThreadId = comment.ThreadId,
                AuthorId = comment.AuthorId,
                ParentId = comment.ParentId,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                UpdateAt = comment.UpdateAt,

                LikesCount = stats.likes,
                DislikesCount = stats.dislikes,
                PopularityScore = stats.score,

                Replies = new List<CommentTreeItemDto>()
            };
        });

        var roots = new List<CommentTreeItemDto>();

        foreach(var comment in comments)
        {
            var currentNode = nodes[comment.Id];

            if(comment.ParentId is null)
            {
                roots.Add(currentNode);
                continue;
            }

            if(nodes.TryGetValue(comment.ParentId.Value, out var parentNode))
            {
                parentNode.Replies.Add(currentNode);
            }
        }

        return roots;
    }

    private static Dictionary<Guid, Comment> BuildComment(List<Comment> comments)
    {
        var commentById = comments.ToDictionary(x => x.Id);

        foreach(var comment in comments)
        {
            if(comment.ParentId is null)
                continue;

            if(commentById.TryGetValue(comment.ParentId.Value, out var parent))
            {
                parent.AddChild(comment);
            }
        }

        return commentById;
    }

    private List<Guid> CollectBranchIds(Comment rootComment)
    {
        var ids = new List<Guid>();
        var iterator = _commentTraversal.CreateDfs(rootComment);

        while (iterator.HasNext())
        {
            var current = iterator.Next();
            ids.Add(current.Id);
        }

        return ids;
    }

    private static Dictionary<Guid, (int likes, int dislikes, int score)> BuildReactionStats(List<Reaction> reactions)
{
    return reactions
        .GroupBy(x => x.CommentId)
        .ToDictionary(
            group => group.Key,
            group =>
            {
                var likes = group.Count(x => x.Type == Domain.Enums.ReactionType.Like);
                var dislikes = group.Count(x => x.Type == Domain.Enums.ReactionType.DisLike);

                return (likes, dislikes, likes - dislikes);
            });
}
}

