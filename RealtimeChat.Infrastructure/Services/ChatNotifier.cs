using Microsoft.AspNetCore.SignalR;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Infrastructure.Hubs;

namespace RealtimeChat.Infrastructure.Services;

/// <summary>
/// Sends chat notifications through SignalR.
/// </summary>
public sealed class ChatNotifier : IChatNotifier
{
    private readonly IHubContext<ChatHub> _hubContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatNotifier"/> class.
    /// </summary>
    /// <param name="hubContext">SignalR hub context.</param>
    public ChatNotifier(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <inheritdoc />
    public Task BroadcastMessageAsync(string user, string message, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message, cancellationToken);
    }

    /// <inheritdoc />
    public Task BroadcastAsync(string method, object?[] args, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync(method, args, cancellationToken);
    }

    /// <inheritdoc />
    public Task SendToUserAsync(string userId, string method, object?[] args, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.User(userId).SendAsync(method, args, cancellationToken);
    }

    /// <inheritdoc />
    public Task SendToRoomAsync(string roomName, string method, object?[] args, CancellationToken cancellationToken)
    {
        return _hubContext.Clients.Group(roomName).SendAsync(method, args, cancellationToken);
    }

    /// <inheritdoc />
    public Task AddToRoomAsync(string connectionId, string roomName, CancellationToken cancellationToken)
    {
        return _hubContext.Groups.AddToGroupAsync(connectionId, roomName, cancellationToken);
    }

    /// <inheritdoc />
    public Task RemoveFromRoomAsync(string connectionId, string roomName, CancellationToken cancellationToken)
    {
        return _hubContext.Groups.RemoveFromGroupAsync(connectionId, roomName, cancellationToken);
    }
}
