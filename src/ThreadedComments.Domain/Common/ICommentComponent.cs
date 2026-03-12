namespace ThreadedComments.Domain.Common;


public interface ICommentComponent
{
    Guid Id { get; }
    IReadOnlyList<ICommentComponent> GetChildren();
    void AddChild(ICommentComponent child);
    void RemoveChild(Guid childId);
}