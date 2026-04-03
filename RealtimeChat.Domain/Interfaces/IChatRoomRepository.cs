using RealtimeChat.Domain.Entities;

namespace RealtimeChat.Domain.Interfaces;

/// <summary>
/// Defines persistence operations for chat rooms.
/// </summary>
public interface IChatRoomRepository
{
    /// <summary>
    /// Gets a room by name.
    /// </summary>
    /// <param name="name">The room name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<ChatRoom?> GetByNameAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new room to the store.
    /// </summary>
    /// <param name="room">The room to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(ChatRoom room, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a user to a room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddMemberAsync(int roomId, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a user from a room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RemoveMemberAsync(int roomId, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks whether a user is a member of a room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<bool> IsMemberAsync(int roomId, string userId, CancellationToken cancellationToken);
}
