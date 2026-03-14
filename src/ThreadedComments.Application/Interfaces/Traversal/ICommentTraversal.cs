using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Application.Interface.Traversal;


public interface ICommentTraversal
{
    ICommentIterator CreateDfs(Comment root);
}