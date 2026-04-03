using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Features.Auth;

namespace RealtimeChat.API.Controllers;

/// <summary>
/// Authentication endpoints.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="mediator">Mediator instance.</param>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="model">Registration payload.</param>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var result = await _mediator.Send(new RegisterCommand(model));
        if (!result.Succeeded)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }

        return Ok(new
        {
            result.Message,
            result.AccessToken,
            result.RefreshToken,
            result.User
        });
    }

    /// <summary>
    /// Logs a user in and returns an access token.
    /// </summary>
    /// <param name="model">Login payload.</param>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var result = await _mediator.Send(new LoginCommand(model));
        if (!result.Succeeded)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }

        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken,
            User = result.User
        });
    }

    /// <summary>
    /// Refreshes the access token.
    /// </summary>
    /// <param name="model">Refresh payload.</param>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshTokenRequestDto model)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(model));
        if (!result.Succeeded)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }

        return Ok(new
        {
            result.AccessToken,
            result.RefreshToken,
            User = result.User
        });
    }

    /// <summary>
    /// Logs out the current user by revoking the refresh token.
    /// </summary>
    /// <param name="model">Logout payload.</param>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequestDto model)
    {
        var result = await _mediator.Send(new LogoutCommand(model));
        if (!result.Succeeded)
        {
            return StatusCode(result.StatusCode, result.Errors);
        }

        return Ok(new { result.Message });
    }
}
