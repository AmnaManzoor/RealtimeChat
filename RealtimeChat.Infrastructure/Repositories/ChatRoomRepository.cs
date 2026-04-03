using Microsoft.EntityFrameworkCore;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;
using RealtimeChat.Infrastructure.Data;

namespace RealtimeChat.Infrastructure.Repositories;

/// <summary>
/// Implements chat room persistence using Entity Framework Core.
/// </summary>
public sealed class ChatRoomRepository : IChatRoomRepository
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatRoomRepository"/> class.
    /// </summary>
    /// <param name="dbContext">Application database context.</param>
    public ChatRoomRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<ChatRoom?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return _dbContext.ChatRooms
            .Include(r => r.Members)
            .SingleOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(ChatRoom room, CancellationToken cancellationToken)
    {
        await _dbContext.ChatRooms.AddAsync(room, cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddMemberAsync(int roomId, string userId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.ChatRoomMembers
            .AnyAsync(m => m.RoomId == roomId && m.UserId == userId, cancellationToken);
        if (!exists)
        {
            await _dbContext.ChatRoomMembers.AddAsync(new ChatRoomMember
            {
                RoomId = roomId,
                UserId = userId
            }, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task RemoveMemberAsync(int roomId, string userId, CancellationToken cancellationToken)
    {
        var membership = await _dbContext.ChatRoomMembers
            .SingleOrDefaultAsync(m => m.RoomId == roomId && m.UserId == userId, cancellationToken);
        if (membership != null)
        {
            _dbContext.ChatRoomMembers.Remove(membership);
        }
    }

    /// <inheritdoc />
    public Task<bool> IsMemberAsync(int roomId, string userId, CancellationToken cancellationToken)
    {
        return _dbContext.ChatRoomMembers
            .AnyAsync(m => m.RoomId == roomId && m.UserId == userId, cancellationToken);
    }
}
