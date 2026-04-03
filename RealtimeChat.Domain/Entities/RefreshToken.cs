namespace RealtimeChat.Domain.Entities;

/// <summary>
/// Represents a refresh token issued to a user.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Gets or sets the unique identifier for the refresh token.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the owning user identifier.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user navigation.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the hashed token value.
    /// </summary>
    public string TokenHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC time the token was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the UTC time the token expires.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the token was revoked.
    /// </summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Gets or sets the token hash that replaced this token.
    /// </summary>
    public string? ReplacedByTokenHash { get; set; }

    /// <summary>
    /// Gets a value indicating whether the token is active.
    /// </summary>
    public bool IsActive => RevokedAt == null && ExpiresAt > DateTime.UtcNow;
}
