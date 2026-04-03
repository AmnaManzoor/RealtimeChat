using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Represents a typing indicator request.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="DisplayName">The user display name.</param>
/// <param name="Payload">The typing payload.</param>
public sealed record TypingCommand(string UserId, string DisplayName, TypingDto Payload) : IRequest;
