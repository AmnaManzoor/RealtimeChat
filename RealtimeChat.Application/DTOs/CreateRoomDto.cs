namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a create room request.
/// </summary>
public class CreateRoomDto
{
    /// <summary>
    /// Gets or sets the room name.
    /// </summary>
    public string RoomName { get; set; } = string.Empty;
}
