namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a chat message payload.
/// </summary>
public class SendMessageDto
{
    /// <summary>
    /// Gets or sets the message content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target room name for room messages.
    /// </summary>
    public string? RoomName { get; set; }

    /// <summary>
    /// Gets or sets the receiver user identifier for direct messages.
    /// </summary>
    public string? ReceiverId { get; set; }
}
