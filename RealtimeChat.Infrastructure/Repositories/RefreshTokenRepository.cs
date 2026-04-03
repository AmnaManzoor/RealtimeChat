using Microsoft.EntityFrameworkCore;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;
using RealtimeChat.Infrastructure.Data;

namespace RealtimeChat.Infrastructure.Repositories;

/// <summary>
/// Implements refresh token persistence using Entity Framework Core.
/// </summary>
public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
    /// </summary>
    /// <param name="dbContext">Application database context.</param>
    public RefreshTokenRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }

    /// <inheritdoc />
    public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken)
    {
        return _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .SingleOrDefaultAsync(rt => rt.TokenHash == tokenHash, cancellationToken);
    }
}
