using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace RealtimeChat.Application;

/// <summary>
/// Provides dependency injection registration for the Application layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers Application services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));

        return services;
    }
}
