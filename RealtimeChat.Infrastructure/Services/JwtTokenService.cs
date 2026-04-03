using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealtimeChat.Infrastructure.Services;

/// <summary>
/// Generates JWT access tokens.
/// </summary>
public sealed class JwtTokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtTokenService"/> class.
    /// </summary>
    /// <param name="jwtOptions">JWT options.</param>
    public JwtTokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    /// <inheritdoc />
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("DisplayName", user.DisplayName ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
