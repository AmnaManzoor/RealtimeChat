namespace RealtimeChat.Domain.Entities;

/// <summary>
/// Represents membership of a user in a chat room.
/// </summary>
public class ChatRoomMember
{
    /// <summary>
    /// Gets or sets the membership identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the room identifier.
    /// </summary>
    public int RoomId { get; set; }

    /// <summary>
    /// Gets or sets the room navigation.
    /// </summary>
    public ChatRoom? Room { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user navigation.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the time the user joined in UTC.
    /// </summary>
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
