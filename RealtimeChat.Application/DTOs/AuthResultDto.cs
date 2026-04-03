namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents the result of an authentication operation.
/// </summary>
public class AuthResultDto
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation succeeded.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code for the result.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets a user-facing message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the user details.
    /// </summary>
    public UserDto? User { get; set; }

    /// <summary>
    /// Gets or sets error messages when the operation fails.
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; set; } = Array.Empty<string>();
}
