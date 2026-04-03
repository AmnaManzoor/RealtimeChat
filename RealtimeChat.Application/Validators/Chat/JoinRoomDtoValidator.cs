using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Chat;

/// <summary>
/// Validates <see cref="JoinRoomDto"/>.
/// </summary>
public sealed class JoinRoomDtoValidator : AbstractValidator<JoinRoomDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JoinRoomDtoValidator"/> class.
    /// </summary>
    public JoinRoomDtoValidator()
    {
        RuleFor(x => x.RoomName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);
    }
}
