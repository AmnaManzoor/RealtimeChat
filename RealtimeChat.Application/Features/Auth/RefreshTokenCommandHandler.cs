using MediatR;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Interfaces;

namespace RealtimeChat.Application.Features.Auth;

/// <summary>
/// Handles refresh token requests.
/// </summary>
public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenCommandHandler"/> class.
    /// </summary>
    /// <param name="authService">Authentication service.</param>
    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    /// <inheritdoc />
    public Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return _authService.RefreshAsync(request.RefreshToken.RefreshToken, cancellationToken);
    }
}
