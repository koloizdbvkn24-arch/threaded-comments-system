using ThreadedComments.Application.Interface.Traversal;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Traversal;


public sealed class CommentTraversal : ICommentTraversal
{
    public ICommentIterator CreateDfs(Comment root)
    {
        return new DfsCommentIterator(root);
    }
}