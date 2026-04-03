namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a refresh token request.
/// </summary>
public class RefreshTokenRequestDto
{
    /// <summary>
    /// Gets or sets the refresh token value.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
