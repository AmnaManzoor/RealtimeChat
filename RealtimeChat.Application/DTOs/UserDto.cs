namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents the user data returned to clients.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    public string? Email { get; set; }
}
