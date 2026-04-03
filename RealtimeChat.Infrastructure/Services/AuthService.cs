using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;
using RealtimeChat.Infrastructure.Options;
using System.Security.Cryptography;
using System.Text;

namespace RealtimeChat.Infrastructure.Services;

/// <summary>
/// Implements authentication operations using ASP.NET Core Identity.
/// </summary>
public sealed class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    /// <param name="userManager">User manager.</param>
    /// <param name="signInManager">Sign-in manager.</param>
    /// <param name="tokenService">Token service.</param>
    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IOptions<JwtOptions> jwtOptions,
        Serilog.ILogger logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _jwtOptions = jwtOptions.Value;
        _logger = logger.ForContext<AuthService>();
    }

    /// <inheritdoc />
    public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName
        };

        _logger.Information("Register attempt for {Email}", registerDto.Email);
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            var tokens = await CreateAndStoreTokensAsync(user, cancellationToken);
            _logger.Information("User registered {UserId}", user.Id);
            return new AuthResultDto
            {
                Succeeded = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "User registered successfully",
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                User = tokens.User
            };
        }

        _logger.Warning("Registration failed for {Email}: {Errors}", registerDto.Email, result.Errors.Select(e => e.Description).ToArray());
        return new AuthResultDto
        {
            Succeeded = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Errors = result.Errors.Select(e => e.Description).ToArray()
        };
    }

    /// <inheritdoc />
    public async Task<AuthResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        _logger.Information("Login attempt for {Email}", loginDto.Email);
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            _logger.Warning("Login failed for {Email}: user not found", loginDto.Email);
            return new AuthResultDto
            {
                Succeeded = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Errors = new[] { "Invalid email or password" }
            };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
        {
            _logger.Warning("Login failed for {Email}: invalid password", loginDto.Email);
            return new AuthResultDto
            {
                Succeeded = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Errors = new[] { "Invalid email or password" }
            };
        }

        var tokens = await CreateAndStoreTokensAsync(user, cancellationToken);
        _logger.Information("User logged in {UserId}", user.Id);
        return new AuthResultDto
        {
            Succeeded = true,
            StatusCode = StatusCodes.Status200OK,
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            User = tokens.User
        };
    }

    /// <inheritdoc />
    public async Task<AuthResultDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return new AuthResultDto
            {
                Succeeded = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = new[] { "Refresh token is required" }
            };
        }

        var tokenHash = HashToken(refreshToken);
        var storedToken = await _unitOfWork.RefreshTokens.GetByHashAsync(tokenHash, cancellationToken);
        if (storedToken == null || !storedToken.IsActive)
        {
            _logger.Warning("Refresh failed: invalid refresh token");
            return new AuthResultDto
            {
                Succeeded = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Errors = new[] { "Refresh token is invalid or expired" }
            };
        }

        var user = storedToken.User ?? await _userManager.Users.SingleOrDefaultAsync(u => u.Id == storedToken.UserId, cancellationToken);
        if (user == null)
        {
            _logger.Warning("Refresh failed: user not found");
            return new AuthResultDto
            {
                Succeeded = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                Errors = new[] { "User not found" }
            };
        }

        var tokens = await RotateRefreshTokenAsync(user, storedToken, cancellationToken);
        _logger.Information("Token refreshed for {UserId}", user.Id);
        return new AuthResultDto
        {
            Succeeded = true,
            StatusCode = StatusCodes.Status200OK,
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            User = tokens.User
        };
    }

    /// <inheritdoc />
    public async Task<AuthResultDto> LogoutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return new AuthResultDto
            {
                Succeeded = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Errors = new[] { "Refresh token is required" }
            };
        }

        var tokenHash = HashToken(refreshToken);
        var storedToken = await _unitOfWork.RefreshTokens.GetByHashAsync(tokenHash, cancellationToken);
        if (storedToken == null)
        {
            _logger.Information("Logout called with unknown refresh token");
            return new AuthResultDto
            {
                Succeeded = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Already logged out"
            };
        }

        if (storedToken.IsActive)
        {
            storedToken.RevokedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        _logger.Information("User logged out {UserId}", storedToken.UserId);
        return new AuthResultDto
        {
            Succeeded = true,
            StatusCode = StatusCodes.Status200OK,
            Message = "Logged out"
        };
    }

    private async Task<(string AccessToken, string RefreshToken, UserDto User)> CreateAndStoreTokensAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = _tokenService.GenerateToken(user);
        var refreshTokenValue = GenerateRefreshToken();
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(refreshTokenValue),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays)
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return (accessToken, refreshTokenValue, new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email
        });
    }

    private async Task<(string AccessToken, string RefreshToken, UserDto User)> RotateRefreshTokenAsync(
        User user,
        RefreshToken currentToken,
        CancellationToken cancellationToken)
    {
        currentToken.RevokedAt = DateTime.UtcNow;

        var newRefreshTokenValue = GenerateRefreshToken();
        var newTokenHash = HashToken(newRefreshTokenValue);
        currentToken.ReplacedByTokenHash = newTokenHash;

        var newRefreshToken = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = newTokenHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays)
        };

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var accessToken = _tokenService.GenerateToken(user);
        return (accessToken, newRefreshTokenValue, new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email
        });
    }

    private static string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}
