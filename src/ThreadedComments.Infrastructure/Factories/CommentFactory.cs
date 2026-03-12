using System;
using ThreadedComments.Application.Interface.Factories;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Factories;


public sealed class CommentFactory : ICommentFactory
{
    public Comment CreateRoot(Guid threadId, Guid authorId, string text)
    {
        return new Comment(
            Guid.NewGuid(),
            threadId,
            authorId,
            parentId: null,
            text,
            DateTime.UtcNow
        );
    }
}