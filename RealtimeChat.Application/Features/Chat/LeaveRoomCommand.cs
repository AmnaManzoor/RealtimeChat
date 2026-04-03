using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Represents a request to leave a chat room.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="DisplayName">The user display name.</param>
/// <param name="ConnectionId">The connection identifier.</param>
/// <param name="Payload">The leave room payload.</param>
public sealed record LeaveRoomCommand(
    string UserId,
    string DisplayName,
    string ConnectionId,
    LeaveRoomDto Payload) : IRequest;
