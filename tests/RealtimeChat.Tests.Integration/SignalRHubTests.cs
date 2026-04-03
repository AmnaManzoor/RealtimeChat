using FluentAssertions;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using RealtimeChat.Application.DTOs;
using System.Net.Http.Json;
using Xunit;

namespace RealtimeChat.Tests.Integration;

public class SignalRHubTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public SignalRHubTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SendMessage_ToRoom_IsReceived()
    {
        var auth = await RegisterAndLoginAsync();
        auth.AccessToken.Should().NotBeNullOrWhiteSpace();

        var messageReceived = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

        var connection = new HubConnectionBuilder()
            .WithUrl(new Uri(_factory.Server.BaseAddress!, "/hubs/chat"), options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(auth.AccessToken)!;
                options.Transports = HttpTransportType.LongPolling;
                options.HttpMessageHandlerFactory = _ => _factory.Server.CreateHandler();
            })
            .Build();

        connection.On<int, string, string, string, string, DateTime>("ReceiveRoomMessage",
            (id, userId, displayName, content, roomName, timestamp) =>
            {
                messageReceived.TrySetResult(content);
            });

        await connection.StartAsync();

        await connection.InvokeAsync("CreateRoom", new CreateRoomDto { RoomName = "general" });
        await connection.InvokeAsync("JoinRoom", new JoinRoomDto { RoomName = "general" });
        await connection.InvokeAsync("SendMessage", new SendMessageDto { Content = "Hello room", RoomName = "general" });

        var completed = await Task.WhenAny(messageReceived.Task, Task.Delay(TimeSpan.FromSeconds(5)));
        completed.Should().Be(messageReceived.Task);
        messageReceived.Task.Result.Should().Be("Hello room");

        await connection.DisposeAsync();
    }

    private async Task<AuthResultDto> RegisterAndLoginAsync()
    {
        var client = _factory.CreateClient();
        var email = $"user{Guid.NewGuid():N}@example.com";
        var password = "Password123!";

        var register = await client.PostAsJsonAsync("/api/auth/register", new RegisterDto
        {
            Email = email,
            Password = password,
            DisplayName = "SignalR User"
        });

        register.EnsureSuccessStatusCode();

        var login = await client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = email,
            Password = password
        });

        login.EnsureSuccessStatusCode();

        var payload = await login.Content.ReadFromJsonAsync<AuthResultDto>();
        return payload!;
    }
}
