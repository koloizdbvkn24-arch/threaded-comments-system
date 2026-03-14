using System.ComponentModel;
using ThreadedComments.Domain.Common;

namespace ThreadedComments.Domain.Entities;


public sealed class Comment : ICommentComponent
{
    private readonly List<Comment> _children = new();
    public Guid Id { get; private set; }
    public Guid AuthorId { get; private set; }
    public Guid ThreadId { get; private set; }
    public Guid? ParentId { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } 
    public DateTime? UpdateAt { get; private set; }

    private Comment(){}

    public Comment(
        Guid id,
        Guid threadId,
        Guid authorId,
        Guid? parentId,
        string text,
        DateTime createdAt
    )
    {
        if(id == Guid.Empty)
            throw new ArgumentException("Comment id cannot be empty", nameof(id));

        if(threadId == Guid.Empty)
            throw new ArgumentException("Thread id cannot be empty", nameof(threadId));

        if(authorId == Guid.Empty)
            throw new ArgumentException("Author id cannot be empty", nameof(authorId));

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Comment text cannot be empty", nameof(text));

        
        Id = id;
        ThreadId = threadId;
        AuthorId = authorId;
        ParentId = parentId;
        Text = text;
        CreatedAt = createdAt;
    }

    public void Edit(string newText, DateTime updateAt)
    {
        if(string.IsNullOrWhiteSpace(newText))
            throw new ArgumentException("New text cannot be empty", nameof(newText));

        Text = newText;
        UpdateAt = updateAt;
    }

    public IReadOnlyList<ICommentComponent> GetChildren()
    {
        return _children.Cast<ICommentComponent>().ToList().AsReadOnly();
    }

    public IReadOnlyList<Comment> Children => _children.AsReadOnly();

    public void AddChild(ICommentComponent child)
    {
        if (child is not Comment commentChild)
            throw new ArgumentException("Child must be of type Comment", nameof(child));

        _children.Add(commentChild);
    }

    public void RemoveChild(Guid childId)
    {
        var child = _children.FirstOrDefault(x => x.Id == childId);
        
        if (child is not null)
        {
            _children.Remove(child);
        }
    }


}