using MediatR;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Interfaces;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Handles leave room requests.
/// </summary>
public sealed class LeaveRoomCommandHandler : IRequestHandler<LeaveRoomCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChatNotifier _chatNotifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeaveRoomCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="chatNotifier">Chat notifier.</param>
    public LeaveRoomCommandHandler(IUnitOfWork unitOfWork, IChatNotifier chatNotifier)
    {
        _unitOfWork = unitOfWork;
        _chatNotifier = chatNotifier;
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(LeaveRoomCommand request, CancellationToken cancellationToken)
    {
        var roomName = request.Payload.RoomName?.Trim();
        if (string.IsNullOrWhiteSpace(roomName))
        {
            throw new InvalidOperationException("Room name is required.");
        }

        var room = await _unitOfWork.ChatRooms.GetByNameAsync(roomName, cancellationToken);
        if (room == null)
        {
            return Unit.Value;
        }

        await _unitOfWork.ChatRooms.RemoveMemberAsync(room.Id, request.UserId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _chatNotifier.RemoveFromRoomAsync(request.ConnectionId, roomName, cancellationToken);
        await _chatNotifier.SendToRoomAsync(roomName, "UserLeftRoom", new object?[]
        {
            request.UserId,
            request.DisplayName,
            roomName
        }, cancellationToken);

        return Unit.Value;
    }
}
