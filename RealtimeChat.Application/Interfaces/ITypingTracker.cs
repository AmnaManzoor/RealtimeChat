namespace RealtimeChat.Application.Interfaces;

/// <summary>
/// Tracks typing indicators and dispatches timeouts.
/// </summary>
public interface ITypingTracker
{
    /// <summary>
    /// Records typing activity for the specified target and handles timeout.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="displayName">The user display name.</param>
    /// <param name="roomName">Optional room name.</param>
    /// <param name="targetUserId">Optional target user identifier for direct chats.</param>
    Task StartTypingAsync(string userId, string displayName, string? roomName, string? targetUserId);
}
