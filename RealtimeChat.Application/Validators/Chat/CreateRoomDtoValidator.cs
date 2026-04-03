using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Chat;

/// <summary>
/// Validates <see cref="CreateRoomDto"/>.
/// </summary>
public sealed class CreateRoomDtoValidator : AbstractValidator<CreateRoomDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoomDtoValidator"/> class.
    /// </summary>
    public CreateRoomDtoValidator()
    {
        RuleFor(x => x.RoomName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);
    }
}
