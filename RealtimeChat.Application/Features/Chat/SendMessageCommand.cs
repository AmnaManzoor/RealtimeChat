using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Represents a request to send a chat message.
/// </summary>
/// <param name="SenderId">The sender identifier.</param>
/// <param name="SenderDisplayName">The sender display name.</param>
/// <param name="Payload">The message payload.</param>
public sealed record SendMessageCommand(string SenderId, string SenderDisplayName, SendMessageDto Payload) : IRequest;
