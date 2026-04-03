namespace RealtimeChat.Application.Interfaces;

/// <summary>
/// Tracks user presence across connections.
/// </summary>
public interface IPresenceTracker
{
    /// <summary>
    /// Registers a user connection and returns true if user became online.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="connectionId">The connection identifier.</param>
    Task<bool> UserConnectedAsync(string userId, string connectionId);

    /// <summary>
    /// Removes a user connection and returns true if user became offline.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="connectionId">The connection identifier.</param>
    Task<bool> UserDisconnectedAsync(string userId, string connectionId);

    /// <summary>
    /// Gets the currently online user identifiers.
    /// </summary>
    IReadOnlyCollection<string> GetOnlineUsers();
}
