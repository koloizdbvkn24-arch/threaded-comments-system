using Microsoft.AspNetCore.Mvc;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.DTOs.Comments;
using ThreadedComments.Application.DTOs.Reactions;

namespace ThreadedComments.WebApi.Controllers;


[ApiController]
[Route("api/comments/{commentId:guid}/reaction")]
public class ReactionController : ControllerBase
{
    private readonly IReactionService _reactionService;

    public ReactionController(
        IReactionService reactionService
    )
    {
        _reactionService = reactionService;
    }

    [HttpPost]
    public async Task<IActionResult> SetReaction(
        Guid commentId,
        [FromBody] SetReactionRequest request,
        CancellationToken ct
    )
    {
        await _reactionService.SetReactionAsync(commentId, request, ct);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveReaction(
        Guid commentId,
        [FromBody] RemoveReactionRequest request,
        CancellationToken ct
    )
    {
        await _reactionService.RemoveReactionAsync(commentId, request, ct);

        return NoContent();
    }
}