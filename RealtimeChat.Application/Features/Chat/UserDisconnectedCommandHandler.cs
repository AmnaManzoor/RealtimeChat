using MediatR;
using RealtimeChat.Application.Interfaces;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Handles user disconnection events.
/// </summary>
public sealed class UserDisconnectedCommandHandler : IRequestHandler<UserDisconnectedCommand>
{
    private readonly IPresenceTracker _presenceTracker;
    private readonly IChatNotifier _chatNotifier;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDisconnectedCommandHandler"/> class.
    /// </summary>
    /// <param name="presenceTracker">Presence tracker.</param>
    /// <param name="chatNotifier">Chat notifier.</param>
    public UserDisconnectedCommandHandler(IPresenceTracker presenceTracker, IChatNotifier chatNotifier)
    {
        _presenceTracker = presenceTracker;
        _chatNotifier = chatNotifier;
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(UserDisconnectedCommand request, CancellationToken cancellationToken)
    {
        var becameOffline = await _presenceTracker.UserDisconnectedAsync(request.UserId, request.ConnectionId);
        if (becameOffline)
        {
            await _chatNotifier.BroadcastAsync("UserOffline", new object?[]
            {
                request.UserId,
                request.DisplayName
            }, cancellationToken);
        }

        return Unit.Value;
    }
}
