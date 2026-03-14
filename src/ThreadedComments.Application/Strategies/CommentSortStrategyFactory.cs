using ThreadedComments.Application.Common.Enums;
using ThreadedComments.Application.Interface.Strategies;

namespace ThreadedComments.Application.Strategies;

public sealed class CommentSortStrategyFactory : ICommentSortStrategyFactory
{
    private readonly SortByNewestStrategy _sortByNewestStrategy;
    private readonly SortByPopularityStrategy _sortByPopularityStrategy;

    public CommentSortStrategyFactory(
        SortByNewestStrategy sortByNewestStrategy,
        SortByPopularityStrategy sortByPopularityStrategy)
    {
        _sortByNewestStrategy = sortByNewestStrategy;
        _sortByPopularityStrategy = sortByPopularityStrategy;
    }

    public ICommentSortStrategy Create(CommentSortBy sortBy)
    {
        return sortBy switch
        {
            CommentSortBy.Popularity => _sortByPopularityStrategy,
            _ => _sortByNewestStrategy
        };
    }
}