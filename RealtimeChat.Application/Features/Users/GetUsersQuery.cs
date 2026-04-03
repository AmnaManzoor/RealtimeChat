using MediatR;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Features.Users;

/// <summary>
/// Requests a list of users for directory search.
/// </summary>
/// <param name="Search">Optional search term.</param>
/// <param name="CurrentUserId">Current user identifier to exclude.</param>
public sealed record GetUsersQuery(string? Search, string? CurrentUserId) : IRequest<IReadOnlyList<UserDto>>;
