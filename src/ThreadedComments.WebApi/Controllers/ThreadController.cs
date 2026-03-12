using Microsoft.AspNetCore.Mvc;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.DTOs.Treads;

namespace ThreadedComments.WebApi.Controllers;

[ApiController]
[Route("api/threads")]
public class ThreadController : ControllerBase
{
    private readonly IThreadService _threadService;

    public ThreadController(IThreadService threadService)
    {
        _threadService = threadService;
    }

    [HttpPost]
    public async Task<ActionResult<ThreadDto>> CreateThreadAsync(
        [FromBody] CreateThreadRequest request,
        CancellationToken ct
    )
    {
        var result = await _threadService.CreateAsync(request, ct);

        return Ok(result);
    }
}