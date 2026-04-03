namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a message reaction payload.
/// </summary>
public class ReactionDto
{
    /// <summary>
    /// Gets or sets the message identifier.
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// Gets or sets the emoji reaction.
    /// </summary>
    public string Emoji { get; set; } = string.Empty;
}
