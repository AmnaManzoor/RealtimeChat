using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Interfaces;

/// <summary>
/// Provides read-only user directory access.
/// </summary>
public interface IUserDirectory
{
    /// <summary>
    /// Retrieves users matching an optional search query.
    /// </summary>
    /// <param name="search">Optional search term.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<UserDto>> GetUsersAsync(string? search, CancellationToken cancellationToken);
}
