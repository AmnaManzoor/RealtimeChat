using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Domain.Interfaces;
using System.Security.Claims;

namespace RealtimeChat.API.Controllers;

/// <summary>
/// Exposes chat history endpoints.
/// </summary>
[ApiController]
[Route("api/chat")]
[Authorize]
public sealed class ChatController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("rooms/{roomName}/messages")]
    public async Task<ActionResult<IReadOnlyCollection<MessageHistoryDto>>> GetRoomMessages(
        string roomName,
        [FromQuery] int take = 50,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(roomName))
        {
            return BadRequest("Room name is required.");
        }

        var room = await _unitOfWork.ChatRooms.GetByNameAsync(roomName.Trim(), cancellationToken);
        if (room == null)
        {
            return NotFound("Room not found.");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var isMember = await _unitOfWork.ChatRooms.IsMemberAsync(room.Id, userId, cancellationToken);
        if (!isMember)
        {
            return Forbid();
        }

        var messages = await _unitOfWork.Messages.GetRoomMessagesAsync(room.Id, take, cancellationToken);
        var response = messages
            .OrderBy(m => m.Timestamp)
            .Select(m => new MessageHistoryDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderName = m.Sender?.DisplayName ?? m.Sender?.Email ?? "Unknown",
                RoomName = room.Name,
                Content = m.Content,
                Timestamp = m.Timestamp
            })
            .ToList();

        return Ok(response);
    }

    [HttpGet("direct/{userId}/messages")]
    public async Task<ActionResult<IReadOnlyCollection<MessageHistoryDto>>> GetDirectMessages(
        string userId,
        [FromQuery] int take = 50,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("User id is required.");
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Unauthorized();
        }

        var messages = await _unitOfWork.Messages.GetDirectMessagesAsync(currentUserId, userId, take, cancellationToken);
        var response = messages
            .OrderBy(m => m.Timestamp)
            .Select(m => new MessageHistoryDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                SenderName = m.Sender?.DisplayName ?? m.Sender?.Email ?? "Unknown",
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                Timestamp = m.Timestamp
            })
            .ToList();

        return Ok(response);
    }
}
