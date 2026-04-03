using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Auth;

/// <summary>
/// Validates <see cref="LogoutRequestDto"/>.
/// </summary>
public sealed class LogoutRequestDtoValidator : AbstractValidator<LogoutRequestDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutRequestDtoValidator"/> class.
    /// </summary>
    public LogoutRequestDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
