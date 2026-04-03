using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Auth;

/// <summary>
/// Validates <see cref="RegisterDto"/>.
/// </summary>
public sealed class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterDtoValidator"/> class.
    /// </summary>
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(100);
    }
}
