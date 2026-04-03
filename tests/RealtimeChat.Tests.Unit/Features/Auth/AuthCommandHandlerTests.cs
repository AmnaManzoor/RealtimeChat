using Moq;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Features.Auth;
using RealtimeChat.Application.Interfaces;
using Xunit;

namespace RealtimeChat.Tests.Unit.Features.Auth;

public class AuthCommandHandlerTests
{
    [Fact]
    public async Task RegisterCommand_InvokesAuthService()
    {
        var authService = new Mock<IAuthService>();
        authService.Setup(a => a.RegisterAsync(It.IsAny<RegisterDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AuthResultDto { Succeeded = true });

        var handler = new RegisterCommandHandler(authService.Object);

        await handler.Handle(new RegisterCommand(new RegisterDto
        {
            Email = "user@example.com",
            Password = "Password123!",
            DisplayName = "User"
        }), CancellationToken.None);

        authService.Verify(a => a.RegisterAsync(It.IsAny<RegisterDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginCommand_InvokesAuthService()
    {
        var authService = new Mock<IAuthService>();
        authService.Setup(a => a.LoginAsync(It.IsAny<LoginDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AuthResultDto { Succeeded = true });

        var handler = new LoginCommandHandler(authService.Object);

        await handler.Handle(new LoginCommand(new LoginDto
        {
            Email = "user@example.com",
            Password = "Password123!"
        }), CancellationToken.None);

        authService.Verify(a => a.LoginAsync(It.IsAny<LoginDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
