using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Interfaces;

/// <summary>
/// Provides authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto">Registration details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<AuthResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);

    /// <summary>
    /// Signs in a user and issues a token.
    /// </summary>
    /// <param name="loginDto">Login credentials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<AuthResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken);

    /// <summary>
    /// Refreshes an access token using a refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token value.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<AuthResultDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Revokes the provided refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token value.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<AuthResultDto> LogoutAsync(string refreshToken, CancellationToken cancellationToken);
}
