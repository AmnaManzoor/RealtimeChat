using FluentAssertions;
using RealtimeChat.Domain.Entities;
using Xunit;

namespace RealtimeChat.Tests.Unit.Domain;

public class RefreshTokenTests
{
    [Fact]
    public void IsActive_ShouldBeTrue_WhenNotRevokedAndNotExpired()
    {
        var token = new RefreshToken
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };

        token.IsActive.Should().BeTrue();
    }

    [Fact]
    public void IsActive_ShouldBeFalse_WhenExpired()
    {
        var token = new RefreshToken
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(-1)
        };

        token.IsActive.Should().BeFalse();
    }

    [Fact]
    public void IsActive_ShouldBeFalse_WhenRevoked()
    {
        var token = new RefreshToken
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            RevokedAt = DateTime.UtcNow
        };

        token.IsActive.Should().BeFalse();
    }
}
