using Microsoft.AspNetCore.Mvc;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.DTOs.Comments;

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
        CancellationToken ct
    )
    {
        var result = await _commentService.GetThreadCommentsAsync(threadId, ct);

        return Ok(result);
    }
}