using MediatR;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Interfaces;

namespace RealtimeChat.Application.Features.Auth;

/// <summary>
/// Handles login requests.
/// </summary>
public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </summary>
    /// <param name="authService">Authentication service.</param>
    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    /// <inheritdoc />
    public Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return _authService.LoginAsync(request.LoginDto, cancellationToken);
    }
}
