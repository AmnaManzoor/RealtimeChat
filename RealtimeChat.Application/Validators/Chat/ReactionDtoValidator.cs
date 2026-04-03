using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Chat;

/// <summary>
/// Validates <see cref="ReactionDto"/>.
/// </summary>
public sealed class ReactionDtoValidator : AbstractValidator<ReactionDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactionDtoValidator"/> class.
    /// </summary>
    public ReactionDtoValidator()
    {
        RuleFor(x => x.MessageId)
            .GreaterThan(0);

        RuleFor(x => x.Emoji)
            .NotEmpty()
            .MaximumLength(10);
    }
}
