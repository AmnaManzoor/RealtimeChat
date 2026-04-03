namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a typing indicator payload.
/// </summary>
public class TypingDto
{
    /// <summary>
    /// Gets or sets the room name for room typing.
    /// </summary>
    public string? RoomName { get; set; }

    /// <summary>
    /// Gets or sets the target user identifier for direct typing.
    /// </summary>
    public string? TargetUserId { get; set; }
}
