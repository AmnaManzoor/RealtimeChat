using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Features.Users;
using System.Security.Claims;

namespace RealtimeChat.API.Controllers;

/// <summary>
/// User directory endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator instance.</param>
    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns user directory entries, optionally filtered by search.
    /// </summary>
    /// <param name="search">Search term.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers([FromQuery] string? search, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var users = await _mediator.Send(new GetUsersQuery(search, currentUserId), cancellationToken);
        return Ok(users);
    }
}
