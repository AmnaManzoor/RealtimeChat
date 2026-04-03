using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;

namespace RealtimeChat.Infrastructure.Services;

/// <summary>
/// Provides access to the identity user directory.
/// </summary>
public sealed class UserDirectory : IUserDirectory
{
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDirectory"/> class.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    public UserDirectory(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserDto>> GetUsersAsync(string? search, CancellationToken cancellationToken)
    {
        var query = _userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(user =>
                (!string.IsNullOrWhiteSpace(user.DisplayName) && EF.Functions.Like(user.DisplayName, $"%{term}%")) ||
                (!string.IsNullOrWhiteSpace(user.Email) && EF.Functions.Like(user.Email, $"%{term}%")));
        }

        return await query
            .OrderBy(user => user.DisplayName ?? user.Email)
            .Select(user => new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email
            })
            .ToListAsync(cancellationToken);
    }
}
