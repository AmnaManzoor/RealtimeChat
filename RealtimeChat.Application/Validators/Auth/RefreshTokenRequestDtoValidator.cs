using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Auth;

/// <summary>
/// Validates <see cref="RefreshTokenRequestDto"/>.
/// </summary>
public sealed class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenRequestDtoValidator"/> class.
    /// </summary>
    public RefreshTokenRequestDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
