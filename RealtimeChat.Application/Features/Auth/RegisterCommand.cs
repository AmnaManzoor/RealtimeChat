using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Auth;

/// <summary>
/// Represents a registration request.
/// </summary>
/// <param name="RegisterDto">The registration payload.</param>
public sealed record RegisterCommand(RegisterDto RegisterDto) : IRequest<AuthResultDto>;
