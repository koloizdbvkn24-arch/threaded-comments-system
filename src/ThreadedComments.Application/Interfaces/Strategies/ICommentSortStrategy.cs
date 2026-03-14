using ThreadedComments.Application.DTOs.Comments;

namespace ThreadedComments.Application.Interface.Strategies;

public interface ICommentSortStrategy
{
    IReadOnlyList<CommentTreeItemDto> Sort(IReadOnlyList<CommentTreeItemDto> comments);
}