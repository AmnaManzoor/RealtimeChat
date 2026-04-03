using RealtimeChat.Application.Interfaces;
using System.Collections.Concurrent;

namespace RealtimeChat.Infrastructure.Services;

/// <summary>
/// Tracks user presence in memory.
/// </summary>
public sealed class PresenceTracker : IPresenceTracker
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _connections = new();

    /// <inheritdoc />
    public Task<bool> UserConnectedAsync(string userId, string connectionId)
    {
        var connections = _connections.GetOrAdd(userId, _ => new HashSet<string>());
        lock (connections)
        {
            var wasOnline = connections.Count > 0;
            connections.Add(connectionId);
            return Task.FromResult(!wasOnline);
        }
    }

    /// <inheritdoc />
    public Task<bool> UserDisconnectedAsync(string userId, string connectionId)
    {
        if (!_connections.TryGetValue(userId, out var connections))
        {
            return Task.FromResult(false);
        }

        lock (connections)
        {
            connections.Remove(connectionId);
            if (connections.Count == 0)
            {
                _connections.TryRemove(userId, out _);
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }

    /// <inheritdoc />
    public IReadOnlyCollection<string> GetOnlineUsers()
    {
        return _connections.Keys.ToArray();
    }
}
