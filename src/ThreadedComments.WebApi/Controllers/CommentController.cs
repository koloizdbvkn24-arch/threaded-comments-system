using Microsoft.AspNetCore.Mvc;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.DTOs.Comments;
using ThreadedComments.Application.Common.Enums;

namespace ThreadedComments.WebApi.Controllers;

[ApiController]
[Route("api/threads/{threadId:guid}/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddRootCommet(
        Guid threadId,
        [FromBody] CreateCommentRequest request,
        CancellationToken ct
    )
    {
        var result = await _commentService.AddRootAsync(threadId, request, ct);

        return Ok(result);
    }

    [HttpPost("{parentCommentId:guid}/replies")]
    public async Task<ActionResult<CommentDto>> AddReplyComment(
        Guid threadId,
        Guid parentCommentId,
        [FromBody] CreateCommentRequest request,
        CancellationToken ct
    )
    {
        var result = await _commentService.AddReplyAsync(threadId, parentCommentId, request, ct);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<CommentTreeItemDto>> GetThreadComments(
        Guid threadId,
        [FromQuery] string? sortBy,
        CancellationToken ct
    )
    {
        var sortMode = ParseSortBy(sortBy);

        var result = await _commentService.GetThreadCommentsAsync(threadId, sortMode, ct);

        return Ok(result);
    }
    
    private static CommentSortBy ParseSortBy(string? sortBy)
    {
        if(string.IsNullOrWhiteSpace(sortBy))
            return CommentSortBy.Newest;

        return sortBy.Trim().ToLowerInvariant() switch
        {
            "popularity" => CommentSortBy.Popularity,
            "newest" => CommentSortBy.Newest,
            _ => CommentSortBy.Newest
        };
    }
}