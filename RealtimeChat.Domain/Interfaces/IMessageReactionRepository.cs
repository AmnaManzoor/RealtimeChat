using RealtimeChat.Domain.Entities;

namespace RealtimeChat.Domain.Interfaces;

/// <summary>
/// Defines persistence operations for message reactions.
/// </summary>
public interface IMessageReactionRepository
{
    /// <summary>
    /// Adds a message reaction to the store.
    /// </summary>
    /// <param name="reaction">The reaction to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(MessageReaction reaction, CancellationToken cancellationToken);
}
