namespace RealtimeChat.Application.Interfaces;

/// <summary>
/// Provides per-user rate limiting capabilities.
/// </summary>
public interface IUserRateLimiter
{
    /// <summary>
    /// Attempts to acquire a rate limit token for the specified action.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="action">The action name.</param>
    /// <param name="limit">Maximum allowed in the window.</param>
    /// <param name="window">The time window.</param>
    bool TryAcquire(string userId, string action, int limit, TimeSpan window);
}
