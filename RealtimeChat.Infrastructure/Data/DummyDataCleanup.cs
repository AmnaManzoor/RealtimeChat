using Microsoft.EntityFrameworkCore;

namespace RealtimeChat.Infrastructure.Data;

/// <summary>
/// Removes demo data from the database.
/// </summary>
public static class DummyDataCleanup
{
    /// <summary>
    /// Deletes chat rooms and related data by room name.
    /// </summary>
    /// <param name="dbContext">Application database context.</param>
    /// <param name="roomNames">Room names to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task RemoveChatRoomsAsync(
        AppDbContext dbContext,
        IEnumerable<string> roomNames,
        CancellationToken cancellationToken)
    {
        var normalizedNames = new HashSet<string>(
            roomNames
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim().ToLowerInvariant()));

        if (normalizedNames.Count == 0)
        {
            return;
        }

        var rooms = await dbContext.ChatRooms
            .Where(room => normalizedNames.Contains(room.Name.ToLower()))
            .ToListAsync(cancellationToken);

        if (rooms.Count == 0)
        {
            return;
        }

        var roomIds = rooms.Select(room => room.Id).ToList();

        var members = await dbContext.ChatRoomMembers
            .Where(member => roomIds.Contains(member.RoomId))
            .ToListAsync(cancellationToken);
        dbContext.ChatRoomMembers.RemoveRange(members);

        var messages = await dbContext.Messages
            .Where(message => message.RoomId.HasValue && roomIds.Contains(message.RoomId.Value))
            .ToListAsync(cancellationToken);
        dbContext.Messages.RemoveRange(messages);

        dbContext.ChatRooms.RemoveRange(rooms);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
