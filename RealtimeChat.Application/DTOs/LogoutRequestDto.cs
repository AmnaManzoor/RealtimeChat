namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a logout request.
/// </summary>
public class LogoutRequestDto
{
    /// <summary>
    /// Gets or sets the refresh token value.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
