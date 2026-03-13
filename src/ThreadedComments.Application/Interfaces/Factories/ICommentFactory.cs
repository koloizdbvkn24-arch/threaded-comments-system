using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Interface.Factories;


public interface ICommentFactory
{
    Comment CreateRoot(Guid threadId, Guid authorId, string text);
    Comment CreateReply(Guid threadId, Guid authorId, Guid parentId, string text);
}