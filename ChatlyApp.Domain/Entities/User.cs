using Microsoft.AspNetCore.Identity;

namespace ChatlyApp.Domain.Entities;

public class User : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
}
