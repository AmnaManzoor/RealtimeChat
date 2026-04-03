using RealtimeChat.Domain.Interfaces;
using RealtimeChat.Infrastructure.Data;

namespace RealtimeChat.Infrastructure.Repositories;

/// <summary>
/// Coordinates repository operations for persistence.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="dbContext">Application database context.</param>
    /// <param name="messageRepository">Message repository.</param>
    public UnitOfWork(
        AppDbContext dbContext,
        IMessageRepository messageRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IChatRoomRepository chatRoomRepository,
        IMessageReactionRepository messageReactionRepository)
    {
        _dbContext = dbContext;
        Messages = messageRepository;
        RefreshTokens = refreshTokenRepository;
        ChatRooms = chatRoomRepository;
        MessageReactions = messageReactionRepository;
    }

    /// <inheritdoc />
    public IMessageRepository Messages { get; }

    /// <inheritdoc />
    public IRefreshTokenRepository RefreshTokens { get; }

    /// <inheritdoc />
    public IChatRoomRepository ChatRooms { get; }

    /// <inheritdoc />
    public IMessageReactionRepository MessageReactions { get; }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
