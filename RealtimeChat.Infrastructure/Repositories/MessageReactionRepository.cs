using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;
using RealtimeChat.Infrastructure.Data;

namespace RealtimeChat.Infrastructure.Repositories;

/// <summary>
/// Implements message reaction persistence using Entity Framework Core.
/// </summary>
public sealed class MessageReactionRepository : IMessageReactionRepository
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageReactionRepository"/> class.
    /// </summary>
    /// <param name="dbContext">Application database context.</param>
    public MessageReactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task AddAsync(MessageReaction reaction, CancellationToken cancellationToken)
    {
        await _dbContext.MessageReactions.AddAsync(reaction, cancellationToken);
    }
}
