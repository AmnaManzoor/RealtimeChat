using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Auth;

/// <summary>
/// Represents a login request.
/// </summary>
/// <param name="LoginDto">The login payload.</param>
public sealed record LoginCommand(LoginDto LoginDto) : IRequest<AuthResultDto>;
