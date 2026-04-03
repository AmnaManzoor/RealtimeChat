using RealtimeChat.Domain.Entities;

namespace RealtimeChat.Domain.Interfaces;

/// <summary>
/// Defines persistence operations for refresh tokens.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Adds a refresh token to the store.
    /// </summary>
    /// <param name="refreshToken">The refresh token to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a refresh token by its hashed value.
    /// </summary>
    /// <param name="tokenHash">The hashed token value.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);
}
