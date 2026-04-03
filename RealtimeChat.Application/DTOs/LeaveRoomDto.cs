namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a leave room request.
/// </summary>
public class LeaveRoomDto
{
    /// <summary>
    /// Gets or sets the room name.
    /// </summary>
    public string RoomName { get; set; } = string.Empty;
}
