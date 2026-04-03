using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Chat;

/// <summary>
/// Validates <see cref="SendMessageDto"/>.
/// </summary>
public sealed class SendMessageDtoValidator : AbstractValidator<SendMessageDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SendMessageDtoValidator"/> class.
    /// </summary>
    public SendMessageDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.RoomName) || !string.IsNullOrWhiteSpace(x.ReceiverId))
            .WithMessage("Either RoomName or ReceiverId must be provided.");
    }
}
