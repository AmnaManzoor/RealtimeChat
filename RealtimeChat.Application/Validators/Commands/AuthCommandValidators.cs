using FluentValidation;
using RealtimeChat.Application.Features.Auth;
using RealtimeChat.Application.Validators.Auth;

namespace RealtimeChat.Application.Validators.Commands;

/// <summary>
/// Validates <see cref="RegisterCommand"/>.
/// </summary>
public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterCommandValidator"/> class.
    /// </summary>
    public RegisterCommandValidator()
    {
        RuleFor(x => x.RegisterDto)
            .SetValidator(new RegisterDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="LoginCommand"/>.
/// </summary>
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandValidator"/> class.
    /// </summary>
    public LoginCommandValidator()
    {
        RuleFor(x => x.LoginDto)
            .SetValidator(new LoginDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="RefreshTokenCommand"/>.
/// </summary>
public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenCommandValidator"/> class.
    /// </summary>
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .SetValidator(new RefreshTokenRequestDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="LogoutCommand"/>.
/// </summary>
public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutCommandValidator"/> class.
    /// </summary>
    public LogoutCommandValidator()
    {
        RuleFor(x => x.LogoutRequest)
            .SetValidator(new LogoutRequestDtoValidator());
    }
}
