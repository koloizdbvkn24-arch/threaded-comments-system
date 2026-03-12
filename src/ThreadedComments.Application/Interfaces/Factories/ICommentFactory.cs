using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Interface.Factories;


public interface ICommentFactory
{
    Comment CreateRoot(Guid threadId, Guid authorId, string text);
}