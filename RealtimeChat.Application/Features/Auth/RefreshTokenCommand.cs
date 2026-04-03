using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Auth;

/// <summary>
/// Represents a refresh token request.
/// </summary>
/// <param name="RefreshToken">The refresh token payload.</param>
public sealed record RefreshTokenCommand(RefreshTokenRequestDto RefreshToken) : IRequest<AuthResultDto>;
