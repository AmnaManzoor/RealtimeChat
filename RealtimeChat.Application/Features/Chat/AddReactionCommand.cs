using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Represents a request to add a reaction to a message.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="DisplayName">The user display name.</param>
/// <param name="Payload">The reaction payload.</param>
public sealed record AddReactionCommand(string UserId, string DisplayName, ReactionDto Payload) : IRequest;
