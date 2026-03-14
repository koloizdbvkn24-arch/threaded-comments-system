using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.Common.Exceptions;
using ThreadedComments.Application.DTOs.Reactions;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Services;


public sealed class ReactionService : IReactionService
{ 
    private readonly ICommentRepository _commentRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IReactionRepository _reactionRepository;

    public ReactionService(
        ICommentRepository commentRepository,
        IAuthorRepository authorRepository,
        IReactionRepository reactionRepository
    )
    {
        _commentRepository = commentRepository;
        _authorRepository = authorRepository;
        _reactionRepository = reactionRepository;
    }
    
    public async Task SetReactionAsync(Guid commentId, SetReactionRequest request, CancellationToken ct)
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

        var existingReaction = await _reactionRepository.GetByCommentAndAuthorAsync(commentId, request.AuthorId, ct);

        if(existingReaction is null)
        {
            var reaction = new Reaction(
                Guid.NewGuid(),
                commentId,
                request.AuthorId,
                request.Type,
                DateTime.UtcNow
            );

            await _reactionRepository.AddAsync(reaction, ct);
            return;
        }

        if(existingReaction.Type == request.Type)
            return;

        existingReaction.ChangeType(request.Type);
        await _reactionRepository.UpdateAsync(existingReaction, ct);
    }

    public async Task RemoveReactionAsync(Guid commentId, RemoveReactionRequest request, CancellationToken ct)
    {
        if(commentId == Guid.Empty)
            throw new ArgumentException("Comment id cannot be empty.", nameof(commentId));

        if(request is null)
            throw new ArgumentNullException(nameof(request));

        var reaction = await _reactionRepository.GetByCommentAndAuthorAsync(commentId, request.AuthorId, ct);

        if(reaction is null)
            throw new NotFoundException("Reaction was not found.");

        await _reactionRepository.DeleteAsync(reaction, ct);
    }
}