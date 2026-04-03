using MediatR;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Represents a user connection event.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="DisplayName">The user display name.</param>
/// <param name="ConnectionId">The connection identifier.</param>
public sealed record UserConnectedCommand(string UserId, string DisplayName, string ConnectionId) : IRequest;
