using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;
using RealtimeChat.Infrastructure.Data;
using RealtimeChat.Infrastructure.Options;
using RealtimeChat.Infrastructure.Repositories;
using RealtimeChat.Infrastructure.Services;

namespace RealtimeChat.Infrastructure;

/// <summary>
/// Provides dependency injection registration for the Infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers Infrastructure services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserDirectory, UserDirectory>();
        services.AddSingleton<IChatNotifier, ChatNotifier>();
        services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
        services.AddScoped<IMessageReactionRepository, MessageReactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IPresenceTracker, PresenceTracker>();
        services.AddSingleton<ITypingTracker, TypingTracker>();
        services.AddSingleton<IUserRateLimiter, UserRateLimiter>();

        return services;
    }
}
