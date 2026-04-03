namespace RealtimeChat.Domain.Entities;

/// <summary>
/// Represents a reaction to a message.
/// </summary>
public class MessageReaction
{
    /// <summary>
    /// Gets or sets the reaction identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the message identifier.
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// Gets or sets the message navigation.
    /// </summary>
    public Message? Message { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user navigation.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the emoji reaction.
    /// </summary>
    public string Emoji { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the time the reaction was created in UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
