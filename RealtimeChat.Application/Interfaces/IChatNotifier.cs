namespace RealtimeChat.Application.Interfaces;

/// <summary>
/// Dispatches chat notifications to connected clients.
/// </summary>
public interface IChatNotifier
{
    /// <summary>
    /// Broadcasts a chat message to all clients.
    /// </summary>
    /// <param name="user">The display name or identifier of the sender.</param>
    /// <param name="message">The message content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task BroadcastMessageAsync(string user, string message, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a payload to all connected clients.
    /// </summary>
    /// <param name="method">The client method name.</param>
    /// <param name="args">Arguments for the client method.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task BroadcastAsync(string method, object?[] args, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a payload to a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="method">The client method name.</param>
    /// <param name="args">Arguments for the client method.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SendToUserAsync(string userId, string method, object?[] args, CancellationToken cancellationToken);

    /// <summary>
    /// Sends a payload to a room group.
    /// </summary>
    /// <param name="roomName">The room group name.</param>
    /// <param name="method">The client method name.</param>
    /// <param name="args">Arguments for the client method.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SendToRoomAsync(string roomName, string method, object?[] args, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a connection to a room group.
    /// </summary>
    /// <param name="connectionId">The connection identifier.</param>
    /// <param name="roomName">The room group name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddToRoomAsync(string connectionId, string roomName, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a connection from a room group.
    /// </summary>
    /// <param name="connectionId">The connection identifier.</param>
    /// <param name="roomName">The room group name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RemoveFromRoomAsync(string connectionId, string roomName, CancellationToken cancellationToken);
}
