using MediatR;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Interfaces;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Handles user connection events.
/// </summary>
public sealed class UserConnectedCommandHandler : IRequestHandler<UserConnectedCommand>
{
    private readonly IPresenceTracker _presenceTracker;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChatNotifier _chatNotifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserConnectedCommandHandler"/> class.
    /// </summary>
    /// <param name="presenceTracker">Presence tracker.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="chatNotifier">Chat notifier.</param>
    public UserConnectedCommandHandler(
        IPresenceTracker presenceTracker,
        IUnitOfWork unitOfWork,
        IChatNotifier chatNotifier)
    {
        _presenceTracker = presenceTracker;
        _unitOfWork = unitOfWork;
        _chatNotifier = chatNotifier;
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(UserConnectedCommand request, CancellationToken cancellationToken)
    {
        var becameOnline = await _presenceTracker.UserConnectedAsync(request.UserId, request.ConnectionId);
        if (becameOnline)
        {
            await _chatNotifier.BroadcastAsync("UserOnline", new object?[]
            {
                request.UserId,
                request.DisplayName
            }, cancellationToken);
        }

        var unreadMessages = await _unitOfWork.Messages.GetUnreadDirectMessagesAsync(request.UserId, cancellationToken);
        foreach (var message in unreadMessages)
        {
            message.IsRead = true;
            message.ReadAt = DateTime.UtcNow;
        }

        if (unreadMessages.Count > 0)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            foreach (var message in unreadMessages)
            {
                await _chatNotifier.SendToUserAsync(message.SenderId, "MessageRead", new object?[]
                {
                    message.Id,
                    request.UserId,
                    message.ReadAt
                }, cancellationToken);
            }
        }

        return Unit.Value;
    }
}
