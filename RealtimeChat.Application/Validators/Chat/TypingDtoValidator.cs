using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Chat;

/// <summary>
/// Validates <see cref="TypingDto"/>.
/// </summary>
public sealed class TypingDtoValidator : AbstractValidator<TypingDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypingDtoValidator"/> class.
    /// </summary>
    public TypingDtoValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.RoomName) || !string.IsNullOrWhiteSpace(x.TargetUserId))
            .WithMessage("Either RoomName or TargetUserId must be provided.");
    }
}
