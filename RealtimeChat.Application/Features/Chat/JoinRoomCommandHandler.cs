using MediatR;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Interfaces;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Handles join room requests.
/// </summary>
public sealed class JoinRoomCommandHandler : IRequestHandler<JoinRoomCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChatNotifier _chatNotifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="JoinRoomCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="chatNotifier">Chat notifier.</param>
    public JoinRoomCommandHandler(IUnitOfWork unitOfWork, IChatNotifier chatNotifier)
    {
        _unitOfWork = unitOfWork;
        _chatNotifier = chatNotifier;
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
    {
        var roomName = request.Payload.RoomName?.Trim();
        if (string.IsNullOrWhiteSpace(roomName))
        {
            throw new InvalidOperationException("Room name is required.");
        }

        var room = await _unitOfWork.ChatRooms.GetByNameAsync(roomName, cancellationToken);
        if (room == null)
        {
            throw new InvalidOperationException("Room does not exist.");
        }

        await _unitOfWork.ChatRooms.AddMemberAsync(room.Id, request.UserId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _chatNotifier.AddToRoomAsync(request.ConnectionId, roomName, cancellationToken);
        await _chatNotifier.SendToRoomAsync(roomName, "UserJoinedRoom", new object?[]
        {
            request.UserId,
            request.DisplayName,
            roomName
        }, cancellationToken);

        return Unit.Value;
    }
}
