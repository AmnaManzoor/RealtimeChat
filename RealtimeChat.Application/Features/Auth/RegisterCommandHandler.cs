using MediatR;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Interfaces;

namespace RealtimeChat.Application.Features.Auth;

/// <summary>
/// Handles registration requests.
/// </summary>
public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResultDto>
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
    /// </summary>
    /// <param name="authService">Authentication service.</param>
    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    /// <inheritdoc />
    public Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return _authService.RegisterAsync(request.RegisterDto, cancellationToken);
    }
}
