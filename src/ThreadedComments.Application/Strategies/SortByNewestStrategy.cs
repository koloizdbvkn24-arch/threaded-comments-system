using ThreadedComments.Application.DTOs.Comments;
using ThreadedComments.Application.Interface.Strategies;

namespace ThreadedComments.Application.Strategies;

public sealed class SortByNewestStrategy : ICommentSortStrategy
{
    public IReadOnlyList<CommentTreeItemDto> Sort(IReadOnlyList<CommentTreeItemDto> comments)
    {
        return comments
            .Select(SortNodeRecursively)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }

    private static CommentTreeItemDto SortNodeRecursively(CommentTreeItemDto node)
    {
        var sortedReplies = node.Replies
            .Select(SortNodeRecursively)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return new CommentTreeItemDto
        {
            Id = node.Id,
            ThreadId = node.ThreadId,
            AuthorId = node.AuthorId,
            ParentId = node.ParentId,
            Text = node.Text,
            CreatedAt = node.CreatedAt,
            UpdateAt = node.UpdateAt,
            LikesCount = node.LikesCount,
            DislikesCount = node.DislikesCount,
            PopularityScore = node.PopularityScore,
            Replies = sortedReplies
        };
    }
}