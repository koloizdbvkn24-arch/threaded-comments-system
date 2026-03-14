using ThreadedComments.Application.Interface.Traversal;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Traversal;


public sealed class DfsCommentIterator : ICommentIterator
{
    private readonly Stack<Comment> _stack = new();

    public DfsCommentIterator(Comment root)
    {
        _stack.Push(root);
    }

    public bool HasNext()
    {
        return _stack.Count > 0;
    }

    public Comment Next()
    {
        if(!HasNext())
            throw new InvalidOperationException("No more comments in traversal.");

        var current = _stack.Pop();

        for(int i = current.Children.Count - 1; i >= 0; i--)
        {
            _stack.Push(current.Children[i]);
        }

        return current;    
    }
}