using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Represents a request to join a chat room.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="DisplayName">The user display name.</param>
/// <param name="ConnectionId">The connection identifier.</param>
/// <param name="Payload">The join room payload.</param>
public sealed record JoinRoomCommand(
    string UserId,
    string DisplayName,
    string ConnectionId,
    JoinRoomDto Payload) : IRequest;
