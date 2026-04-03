using RealtimeChat.Application.Interfaces;
using System.Collections.Concurrent;

namespace RealtimeChat.Infrastructure.Services;

/// <summary>
/// Provides in-memory per-user rate limiting.
/// </summary>
public sealed class UserRateLimiter : IUserRateLimiter
{
    private readonly ConcurrentDictionary<string, ConcurrentQueue<DateTime>> _actions = new();

    /// <inheritdoc />
    public bool TryAcquire(string userId, string action, int limit, TimeSpan window)
    {
        var key = $"{action}:{userId}";
        var queue = _actions.GetOrAdd(key, _ => new ConcurrentQueue<DateTime>());
        var now = DateTime.UtcNow;

        lock (queue)
        {
            while (queue.TryPeek(out var timestamp) && now - timestamp > window)
            {
                queue.TryDequeue(out _);
            }

            if (queue.Count >= limit)
            {
                return false;
            }

            queue.Enqueue(now);
            return true;
        }
    }
}
