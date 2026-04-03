using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Auth;

/// <summary>
/// Represents a logout request.
/// </summary>
/// <param name="LogoutRequest">The logout payload.</param>
public sealed record LogoutCommand(LogoutRequestDto LogoutRequest) : IRequest<AuthResultDto>;
