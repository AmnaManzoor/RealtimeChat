using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace RealtimeChat.Infrastructure.Services;

/// <summary>
/// Resolves SignalR user identifiers from JWT claims.
/// </summary>
public sealed class NameIdentifierUserIdProvider : IUserIdProvider
{
    /// <inheritdoc />
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? connection.User?.FindFirstValue("sub");
    }
}
