using ThreadedComments.Application.Common.Enums;

namespace ThreadedComments.Application.Interface.Strategies;

public interface ICommentSortStrategyFactory
{
    ICommentSortStrategy Create(CommentSortBy sortBy);
}