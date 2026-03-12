using Microsoft.AspNetCore.Mvc;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.DTOs.Authors;

namespace ThreadedComments.WebApi.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authortService;

    public AuthorController(IAuthorService authorService)
    {
        _authortService = authorService;
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> AddRootCommet(
        [FromBody] CreateAuthorRequest request,
        CancellationToken ct
    )
    {
        var result = await _authortService.CreateAsync(request, ct);

        return Ok(result);
    }
}