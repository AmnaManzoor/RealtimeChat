namespace RealtimeChat.Domain.Interfaces;

/// <summary>
/// Coordinates persistence changes across repositories.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Gets the message repository.
    /// </summary>
    IMessageRepository Messages { get; }

    /// <summary>
    /// Gets the refresh token repository.
    /// </summary>
    IRefreshTokenRepository RefreshTokens { get; }

    /// <summary>
    /// Gets the chat room repository.
    /// </summary>
    IChatRoomRepository ChatRooms { get; }

    /// <summary>
    /// Gets the message reaction repository.
    /// </summary>
    IMessageReactionRepository MessageReactions { get; }

    /// <summary>
    /// Persists all changes to the data store.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
