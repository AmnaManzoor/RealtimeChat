using MediatR;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Handles chat message broadcasts.
/// </summary>
public sealed class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>
{
    private readonly IChatNotifier _chatNotifier;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendMessageCommandHandler"/> class.
    /// </summary>
    /// <param name="chatNotifier">Chat notification service.</param>
    public SendMessageCommandHandler(IChatNotifier chatNotifier, IUnitOfWork unitOfWork, Serilog.ILogger logger)
    {
        _chatNotifier = chatNotifier;
        _unitOfWork = unitOfWork;
        _logger = logger.ForContext<SendMessageCommandHandler>();
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        if (string.IsNullOrWhiteSpace(payload.Content))
        {
            throw new InvalidOperationException("Message content is required.");
        }

        if (!string.IsNullOrWhiteSpace(payload.RoomName))
        {
            var room = await _unitOfWork.ChatRooms.GetByNameAsync(payload.RoomName, cancellationToken);
            if (room == null)
            {
                throw new InvalidOperationException("Room does not exist.");
            }

            var isMember = await _unitOfWork.ChatRooms.IsMemberAsync(room.Id, request.SenderId, cancellationToken);
            if (!isMember)
            {
                throw new InvalidOperationException("User is not a member of the room.");
            }

            var message = new Message
            {
                Content = payload.Content,
                SenderId = request.SenderId,
                RoomId = room.Id
            };

            await _unitOfWork.Messages.AddAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Information(
                "Room message sent {MessageId} by {UserId} in {RoomId}",
                message.Id,
                request.SenderId,
                room.Id);

            await _chatNotifier.SendToRoomAsync(payload.RoomName, "ReceiveRoomMessage", new object?[]
            {
                message.Id,
                request.SenderId,
                request.SenderDisplayName,
                payload.Content,
                payload.RoomName,
                message.Timestamp
            }, cancellationToken);

            return Unit.Value;
        }

        if (string.IsNullOrWhiteSpace(payload.ReceiverId))
        {
            throw new InvalidOperationException("Receiver is required for direct messages.");
        }

        var directMessage = new Message
        {
            Content = payload.Content,
            SenderId = request.SenderId,
            ReceiverId = payload.ReceiverId
        };

        await _unitOfWork.Messages.AddAsync(directMessage, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information(
            "Direct message sent {MessageId} from {UserId} to {ReceiverId}",
            directMessage.Id,
            request.SenderId,
            payload.ReceiverId);

        var args = new object?[]
        {
            directMessage.Id,
            request.SenderId,
            request.SenderDisplayName,
            payload.ReceiverId,
            payload.Content,
            directMessage.Timestamp
        };

        await _chatNotifier.SendToUserAsync(payload.ReceiverId, "ReceiveMessage", args, cancellationToken);
        await _chatNotifier.SendToUserAsync(request.SenderId, "ReceiveMessage", args, cancellationToken);

        return Unit.Value;
    }
}
