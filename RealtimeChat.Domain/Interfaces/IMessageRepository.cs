using RealtimeChat.Domain.Entities;

namespace RealtimeChat.Domain.Interfaces;

/// <summary>
/// Defines persistence operations for chat messages.
/// </summary>
public interface IMessageRepository
{
    /// <summary>
    /// Adds a message to the store.
    /// </summary>
    /// <param name="message">The message to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(Message message, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a message by identifier.
    /// </summary>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Message?> GetByIdAsync(int messageId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets unread direct messages for the specified user.
    /// </summary>
    /// <param name="userId">The recipient user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyCollection<Message>> GetUnreadDirectMessagesAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets recent room messages for a room.
    /// </summary>
    /// <param name="roomId">The room identifier.</param>
    /// <param name="take">The number of messages to return.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyCollection<Message>> GetRoomMessagesAsync(int roomId, int take, CancellationToken cancellationToken);

    /// <summary>
    /// Gets recent direct messages between two users.
    /// </summary>
    /// <param name="userId">The current user identifier.</param>
    /// <param name="otherUserId">The other user identifier.</param>
    /// <param name="take">The number of messages to return.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyCollection<Message>> GetDirectMessagesAsync(string userId, string otherUserId, int take, CancellationToken cancellationToken);
}
