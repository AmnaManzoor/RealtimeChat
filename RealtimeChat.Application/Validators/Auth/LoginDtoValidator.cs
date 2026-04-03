using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Auth;

/// <summary>
/// Validates <see cref="LoginDto"/>.
/// </summary>
public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginDtoValidator"/> class.
    /// </summary>
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
