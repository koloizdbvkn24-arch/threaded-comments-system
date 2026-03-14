using Microsoft.AspNetCore.Mvc;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.DTOs.Comments;

namespace ThreadedComments.WebApi.Controllers;


[ApiController]
[Route("api/comments/{commentId:guid}")]
public class CommentManagerController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentManagerController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost("update")]
    public async Task<ActionResult<CommentDto>> EditComment(
        Guid commentId,
        [FromBody] EditCommentRequest request,
        CancellationToken ct
    )
    {
        var result = await _commentService.EditCommentAsync(commentId, request, ct);

        return Ok(result);
    }

    [HttpDelete("delete-brench")]
    public async Task<ActionResult> RemoveCommentBrabch(
        Guid commentId,
        [FromBody] DeleteCommentBranchRequest request,
        CancellationToken ct
    )
    {
        await _commentService.DeleteCommentAsync(commentId, request, ct);

        return NoContent();
    }
}
