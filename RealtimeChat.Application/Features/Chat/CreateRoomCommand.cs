using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Represents a request to create a chat room.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="DisplayName">The user display name.</param>
/// <param name="Payload">The create room payload.</param>
public sealed record CreateRoomCommand(string UserId, string DisplayName, CreateRoomDto Payload) : IRequest;
