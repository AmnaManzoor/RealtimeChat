namespace RealtimeChat.Application.DTOs;

/// <summary>
/// Represents a chat message for history retrieval.
/// </summary>
public sealed record MessageHistoryDto
{
    public int Id { get; init; }
    public string SenderId { get; init; } = string.Empty;
    public string SenderName { get; init; } = string.Empty;
    public string? ReceiverId { get; init; }
    public string? RoomName { get; init; }
    public string Content { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}
