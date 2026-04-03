using MediatR;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Handles message reaction requests.
/// </summary>
public sealed class AddReactionCommandHandler : IRequestHandler<AddReactionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChatNotifier _chatNotifier;
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddReactionCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="chatNotifier">Chat notifier.</param>
    public AddReactionCommandHandler(IUnitOfWork unitOfWork, IChatNotifier chatNotifier, Serilog.ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _chatNotifier = chatNotifier;
        _logger = logger.ForContext<AddReactionCommandHandler>();
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(AddReactionCommand request, CancellationToken cancellationToken)
    {
        var emoji = request.Payload.Emoji?.Trim();
        if (string.IsNullOrWhiteSpace(emoji))
        {
            throw new InvalidOperationException("Emoji is required.");
        }

        var message = await _unitOfWork.Messages.GetByIdAsync(request.Payload.MessageId, cancellationToken);
        if (message == null)
        {
            throw new InvalidOperationException("Message not found.");
        }

        var reaction = new MessageReaction
        {
            MessageId = message.Id,
            UserId = request.UserId,
            Emoji = emoji
        };

        await _unitOfWork.MessageReactions.AddAsync(reaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information(
            "Reaction added {MessageId} by {UserId} emoji {Emoji}",
            message.Id,
            request.UserId,
            emoji);

        var args = new object?[]
        {
            message.Id,
            request.UserId,
            request.DisplayName,
            emoji
        };

        if (message.RoomId.HasValue && message.Room != null)
        {
            await _chatNotifier.SendToRoomAsync(message.Room.Name, "MessageReactionAdded", args, cancellationToken);
            return Unit.Value;
        }

        if (!string.IsNullOrWhiteSpace(message.ReceiverId))
        {
            await _chatNotifier.SendToUserAsync(message.ReceiverId, "MessageReactionAdded", args, cancellationToken);
        }

        await _chatNotifier.SendToUserAsync(message.SenderId, "MessageReactionAdded", args, cancellationToken);

        return Unit.Value;
    }
}
