using MediatR;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Interfaces;

namespace RealtimeChat.Application.Features.Auth;

/// <summary>
/// Handles logout requests.
/// </summary>
public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, AuthResultDto>
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutCommandHandler"/> class.
    /// </summary>
    /// <param name="authService">Authentication service.</param>
    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    /// <inheritdoc />
    public Task<AuthResultDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return _authService.LogoutAsync(request.LogoutRequest.RefreshToken, cancellationToken);
    }
}
