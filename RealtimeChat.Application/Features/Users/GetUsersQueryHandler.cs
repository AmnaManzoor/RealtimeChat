using MediatR;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Interfaces;

namespace RealtimeChat.Application.Features.Users;

/// <summary>
/// Handles directory user queries.
/// </summary>
public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly IUserDirectory _userDirectory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersQueryHandler"/> class.
    /// </summary>
    /// <param name="userDirectory">User directory service.</param>
    public GetUsersQueryHandler(IUserDirectory userDirectory)
    {
        _userDirectory = userDirectory;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userDirectory.GetUsersAsync(request.Search, cancellationToken);
        if (string.IsNullOrWhiteSpace(request.CurrentUserId))
        {
            return users;
        }

        return users.Where(user => user.Id != request.CurrentUserId).ToList();
    }
}
