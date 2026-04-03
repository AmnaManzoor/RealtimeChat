namespace RealtimeChat.Infrastructure.Options;

/// <summary>
/// Represents JWT configuration settings.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Gets or sets the signing key.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token issuer.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token audience.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the access token lifetime in minutes.
    /// </summary>
    public int AccessTokenMinutes { get; set; } = 15;

    /// <summary>
    /// Gets or sets the refresh token lifetime in days.
    /// </summary>
    public int RefreshTokenDays { get; set; } = 7;
}
