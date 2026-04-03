using RealtimeChat.Application.Interfaces;
using System.Collections.Concurrent;

namespace RealtimeChat.Infrastructure.Services;

/// <summary>
/// Tracks typing indicators and handles timeouts.
/// </summary>
public sealed class TypingTracker : ITypingTracker
{
    private readonly IChatNotifier _chatNotifier;
    private readonly ConcurrentDictionary<string, Timer> _timers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TypingTracker"/> class.
    /// </summary>
    /// <param name="chatNotifier">Chat notifier.</param>
    public TypingTracker(IChatNotifier chatNotifier)
    {
        _chatNotifier = chatNotifier;
    }

    /// <inheritdoc />
    public Task StartTypingAsync(string userId, string displayName, string? roomName, string? targetUserId)
    {
        var key = $"{roomName ?? string.Empty}:{targetUserId ?? string.Empty}:{userId}";
        return HandleTypingAsync(key, userId, displayName, roomName, targetUserId);
    }

    private async Task HandleTypingAsync(string key, string userId, string displayName, string? roomName, string? targetUserId)
    {
        await NotifyTypingAsync(userId, displayName, roomName, targetUserId);

        if (_timers.TryGetValue(key, out var existing))
        {
            existing.Change(TimeSpan.FromSeconds(3), Timeout.InfiniteTimeSpan);
            return;
        }

        var timer = new Timer(async _ =>
        {
            await NotifyStoppedTypingAsync(userId, displayName, roomName, targetUserId);
            if (_timers.TryRemove(key, out var removed))
            {
                removed.Dispose();
            }
        }, null, TimeSpan.FromSeconds(3), Timeout.InfiniteTimeSpan);

        _timers[key] = timer;
    }

    private Task NotifyTypingAsync(string userId, string displayName, string? roomName, string? targetUserId)
    {
        var args = new object?[] { userId, displayName, roomName };
        if (!string.IsNullOrWhiteSpace(roomName))
        {
            return _chatNotifier.SendToRoomAsync(roomName, "UserTyping", args, CancellationToken.None);
        }

        if (!string.IsNullOrWhiteSpace(targetUserId))
        {
            return _chatNotifier.SendToUserAsync(targetUserId, "UserTyping", args, CancellationToken.None);
        }

        return _chatNotifier.BroadcastAsync("UserTyping", args, CancellationToken.None);
    }

    private Task NotifyStoppedTypingAsync(string userId, string displayName, string? roomName, string? targetUserId)
    {
        var args = new object?[] { userId, displayName, roomName };
        if (!string.IsNullOrWhiteSpace(roomName))
        {
            return _chatNotifier.SendToRoomAsync(roomName, "UserStoppedTyping", args, CancellationToken.None);
        }

        if (!string.IsNullOrWhiteSpace(targetUserId))
        {
            return _chatNotifier.SendToUserAsync(targetUserId, "UserStoppedTyping", args, CancellationToken.None);
        }

        return _chatNotifier.BroadcastAsync("UserStoppedTyping", args, CancellationToken.None);
    }
}
