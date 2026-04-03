namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a join room request.
/// </summary>
public class JoinRoomDto
{
    /// <summary>
    /// Gets or sets the room name.
    /// </summary>
    public string RoomName { get; set; } = string.Empty;
}
