using Microsoft.AspNetCore.Identity;

namespace RealtimeChat.Domain.Entities;

/// <summary>
/// Represents an authenticated user of the system.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets the display name shown to other users.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the avatar URL for the user's profile.
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Gets or sets the user's refresh tokens.
    /// </summary>
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
