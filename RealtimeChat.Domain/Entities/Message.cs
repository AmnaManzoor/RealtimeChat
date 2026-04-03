namespace RealtimeChat.Domain.Entities;

/// <summary>
/// Represents a message between users or within a room.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the unique identifier for the message.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the message content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time the message was created in UTC.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the sender user identifier.
    /// </summary>
    public string SenderId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sender navigation.
    /// </summary>
    public User? Sender { get; set; }

    /// <summary>
    /// Gets or sets the receiver user identifier.
    /// </summary>
    public string? ReceiverId { get; set; }

    /// <summary>
    /// Gets or sets the receiver navigation.
    /// </summary>
    public User? Receiver { get; set; }

    /// <summary>
    /// Gets or sets the room identifier.
    /// </summary>
    public int? RoomId { get; set; }

    /// <summary>
    /// Gets or sets the room navigation.
    /// </summary>
    public ChatRoom? Room { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message has been read.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Gets or sets the time the message was read in UTC.
    /// </summary>
    public DateTime? ReadAt { get; set; }
}
