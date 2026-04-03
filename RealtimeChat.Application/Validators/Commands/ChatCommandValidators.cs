using FluentValidation;
using RealtimeChat.Application.Features.Chat;
using RealtimeChat.Application.Validators.Chat;

namespace RealtimeChat.Application.Validators.Commands;

/// <summary>
/// Validates <see cref="SendMessageCommand"/>.
/// </summary>
public sealed class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SendMessageCommandValidator"/> class.
    /// </summary>
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.Payload)
            .SetValidator(new SendMessageDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="CreateRoomCommand"/>.
/// </summary>
public sealed class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoomCommandValidator"/> class.
    /// </summary>
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Payload)
            .SetValidator(new CreateRoomDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="JoinRoomCommand"/>.
/// </summary>
public sealed class JoinRoomCommandValidator : AbstractValidator<JoinRoomCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JoinRoomCommandValidator"/> class.
    /// </summary>
    public JoinRoomCommandValidator()
    {
        RuleFor(x => x.Payload)
            .SetValidator(new JoinRoomDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="LeaveRoomCommand"/>.
/// </summary>
public sealed class LeaveRoomCommandValidator : AbstractValidator<LeaveRoomCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LeaveRoomCommandValidator"/> class.
    /// </summary>
    public LeaveRoomCommandValidator()
    {
        RuleFor(x => x.Payload)
            .SetValidator(new LeaveRoomDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="TypingCommand"/>.
/// </summary>
public sealed class TypingCommandValidator : AbstractValidator<TypingCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypingCommandValidator"/> class.
    /// </summary>
    public TypingCommandValidator()
    {
        RuleFor(x => x.Payload)
            .SetValidator(new TypingDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="AddReactionCommand"/>.
/// </summary>
public sealed class AddReactionCommandValidator : AbstractValidator<AddReactionCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddReactionCommandValidator"/> class.
    /// </summary>
    public AddReactionCommandValidator()
    {
        RuleFor(x => x.Payload)
            .SetValidator(new ReactionDtoValidator());
    }
}
