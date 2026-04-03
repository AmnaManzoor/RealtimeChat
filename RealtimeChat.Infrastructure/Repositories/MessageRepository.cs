using Microsoft.EntityFrameworkCore;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;
using RealtimeChat.Infrastructure.Data;

namespace RealtimeChat.Infrastructure.Repositories;

/// <summary>
/// Implements message persistence using Entity Framework Core.
/// </summary>
public sealed class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageRepository"/> class.
    /// </summary>
    /// <param name="dbContext">Application database context.</param>
    public MessageRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task AddAsync(Message message, CancellationToken cancellationToken)
    {
        await _dbContext.Messages.AddAsync(message, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Message?> GetByIdAsync(int messageId, CancellationToken cancellationToken)
    {
        return _dbContext.Messages
            .Include(m => m.Room)
            .SingleOrDefaultAsync(m => m.Id == messageId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Message>> GetUnreadDirectMessagesAsync(string userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Messages
            .Where(m => m.ReceiverId == userId && !m.IsRead)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Message>> GetRoomMessagesAsync(int roomId, int take, CancellationToken cancellationToken)
    {
        return await _dbContext.Messages
            .AsNoTracking()
            .Include(m => m.Sender)
            .Where(m => m.RoomId == roomId)
            .OrderByDescending(m => m.Timestamp)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Message>> GetDirectMessagesAsync(
        string userId,
        string otherUserId,
        int take,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Messages
            .AsNoTracking()
            .Include(m => m.Sender)
            .Where(m =>
                (m.SenderId == userId && m.ReceiverId == otherUserId)
                || (m.SenderId == otherUserId && m.ReceiverId == userId))
            .OrderByDescending(m => m.Timestamp)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}
