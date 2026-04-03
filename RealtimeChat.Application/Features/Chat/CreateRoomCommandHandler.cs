using MediatR;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Handles create room requests.
/// </summary>
public sealed class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChatNotifier _chatNotifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoomCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="chatNotifier">Chat notifier.</param>
    public CreateRoomCommandHandler(IUnitOfWork unitOfWork, IChatNotifier chatNotifier)
    {
        _unitOfWork = unitOfWork;
        _chatNotifier = chatNotifier;
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var roomName = request.Payload.RoomName?.Trim();
        if (string.IsNullOrWhiteSpace(roomName))
        {
            throw new InvalidOperationException("Room name is required.");
        }

        var existing = await _unitOfWork.ChatRooms.GetByNameAsync(roomName, cancellationToken);
        if (existing != null)
        {
            return Unit.Value;
        }

        var room = new ChatRoom
        {
            Name = roomName,
            CreatedById = request.UserId
        };

        await _unitOfWork.ChatRooms.AddAsync(room, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _chatNotifier.BroadcastAsync("RoomCreated", new object?[]
        {
            room.Id,
            room.Name,
            request.UserId,
            request.DisplayName
        }, cancellationToken);

        return Unit.Value;
    }
}
