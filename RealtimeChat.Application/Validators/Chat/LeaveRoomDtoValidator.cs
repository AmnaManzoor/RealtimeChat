using FluentValidation;
using RealtimeChat.Application.DTOs;

namespace RealtimeChat.Application.Validators.Chat;

/// <summary>
/// Validates <see cref="LeaveRoomDto"/>.
/// </summary>
public sealed class LeaveRoomDtoValidator : AbstractValidator<LeaveRoomDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LeaveRoomDtoValidator"/> class.
    /// </summary>
    public LeaveRoomDtoValidator()
    {
        RuleFor(x => x.RoomName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);
    }
}
