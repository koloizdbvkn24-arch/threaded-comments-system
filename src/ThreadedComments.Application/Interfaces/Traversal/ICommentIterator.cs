using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Interface.Traversal;


public interface ICommentIterator
{
    bool HasNext();
    Comment Next();
}