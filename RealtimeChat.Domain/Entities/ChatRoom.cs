namespace RealtimeChat.Domain.Entities;

/// <summary>
/// Represents a chat room that users can join.
/// </summary>
public class ChatRoom
{
    /// <summary>
    /// Gets or sets the room identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the room name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the room creation time in UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the creator user identifier.
    /// </summary>
    public string CreatedById { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creator navigation.
    /// </summary>
    public User? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the room members.
    /// </summary>
    public ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
}
