using RealtimeChat.Domain.Entities;

namespace RealtimeChat.Application.Interfaces;

/// <summary>
/// Generates access tokens for authenticated users.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Creates a JWT access token for the specified user.
    /// </summary>
    /// <param name="user">The user to generate the token for.</param>
    /// <returns>The signed JWT token.</returns>
    string GenerateToken(User user);
}
