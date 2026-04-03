using MediatR;
using RealtimeChat.Application.Interfaces;

namespace RealtimeChat.Application.Features.Chat;

/// <summary>
/// Handles typing indicator requests.
/// </summary>
public sealed class TypingCommandHandler : IRequestHandler<TypingCommand>
{
    private readonly ITypingTracker _typingTracker;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypingCommandHandler"/> class.
    /// </summary>
    /// <param name="typingTracker">Typing tracker.</param>
    public TypingCommandHandler(ITypingTracker typingTracker)
    {
        _typingTracker = typingTracker;
    }

    /// <inheritdoc />
    public async Task<Unit> Handle(TypingCommand request, CancellationToken cancellationToken)
    {
        await _typingTracker.StartTypingAsync(
            request.UserId,
            request.DisplayName,
            request.Payload.RoomName,
            request.Payload.TargetUserId);

        return Unit.Value;
    }
}
