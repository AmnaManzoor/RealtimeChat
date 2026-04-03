using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Application.Features.Chat;
using System.Security.Claims;

namespace RealtimeChat.Infrastructure.Hubs;

/// <summary>
/// SignalR hub for real-time chat interactions.
/// </summary>
[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IUserRateLimiter _rateLimiter;
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatHub"/> class.
    /// </summary>
    /// <param name="mediator">Mediator for dispatching commands.</param>
    public ChatHub(IMediator mediator, IUserRateLimiter rateLimiter, Serilog.ILogger logger)
    {
        _mediator = mediator;
        _rateLimiter = rateLimiter;
        _logger = logger.ForContext<ChatHub>();
    }

    /// <summary>
    /// Sends a chat message.
    /// </summary>
    /// <param name="payload">The message payload.</param>
    public Task SendMessage(SendMessageDto payload)
    {
        var userId = Context.UserIdentifier ?? string.Empty;
        var displayName = GetDisplayName();
        Console.WriteLine($"Message from {displayName} ({userId}) to room {payload.RoomName ?? "direct"} receiver {payload.ReceiverId ?? "n/a"}");
        var allowed = _rateLimiter.TryAcquire(userId, "SendMessage", 30, TimeSpan.FromMinutes(1));
        if (!allowed)
        {
            _logger.Warning("Rate limit exceeded for {UserId}", userId);
            throw new HubException("Rate limit exceeded. Try again later.");
        }
        _logger.Information("SendMessage invoked by {UserId} room {RoomName} receiver {ReceiverId}", userId, payload.RoomName, payload.ReceiverId);
        return _mediator.Send(new SendMessageCommand(userId, displayName, payload));
    }

    /// <summary>
    /// Creates a new chat room.
    /// </summary>
    /// <param name="payload">The room payload.</param>
    public Task CreateRoom(CreateRoomDto payload)
    {
        var userId = Context.UserIdentifier ?? string.Empty;
        var displayName = GetDisplayName();
        return _mediator.Send(new CreateRoomCommand(userId, displayName, payload));
    }

    /// <summary>
    /// Joins a chat room.
    /// </summary>
    /// <param name="payload">The join room payload.</param>
    public Task JoinRoom(JoinRoomDto payload)
    {
        var userId = Context.UserIdentifier ?? string.Empty;
        var displayName = GetDisplayName();
        return _mediator.Send(new JoinRoomCommand(userId, displayName, Context.ConnectionId, payload));
    }

    /// <summary>
    /// Leaves a chat room.
    /// </summary>
    /// <param name="payload">The leave room payload.</param>
    public Task LeaveRoom(LeaveRoomDto payload)
    {
        var userId = Context.UserIdentifier ?? string.Empty;
        var displayName = GetDisplayName();
        return _mediator.Send(new LeaveRoomCommand(userId, displayName, Context.ConnectionId, payload));
    }

    /// <summary>
    /// Emits a typing indicator.
    /// </summary>
    /// <param name="payload">The typing payload.</param>
    public Task Typing(TypingDto payload)
    {
        var userId = Context.UserIdentifier ?? string.Empty;
        var displayName = GetDisplayName();
        return _mediator.Send(new TypingCommand(userId, displayName, payload));
    }

    /// <summary>
    /// Adds a reaction to a message.
    /// </summary>
    /// <param name="payload">The reaction payload.</param>
    public Task AddReaction(ReactionDto payload)
    {
        var userId = Context.UserIdentifier ?? string.Empty;
        var displayName = GetDisplayName();
        return _mediator.Send(new AddReactionCommand(userId, displayName, payload));
    }

    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier ?? string.Empty;
        var displayName = GetDisplayName();
        _logger.Information("SignalR connected {UserId} {ConnectionId}", userId, Context.ConnectionId);
        await _mediator.Send(new UserConnectedCommand(userId, displayName, Context.ConnectionId));
        await base.OnConnectedAsync();
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier ?? string.Empty;
        var displayName = GetDisplayName();
        _logger.Information("SignalR disconnected {UserId} {ConnectionId}", userId, Context.ConnectionId);
        await _mediator.Send(new UserDisconnectedCommand(userId, displayName, Context.ConnectionId));
        await base.OnDisconnectedAsync(exception);
    }

    private string GetDisplayName()
    {
        return Context.User?.FindFirstValue("DisplayName")
            ?? Context.User?.FindFirstValue(ClaimTypes.Name)
            ?? Context.UserIdentifier
            ?? "Unknown";
    }
}
