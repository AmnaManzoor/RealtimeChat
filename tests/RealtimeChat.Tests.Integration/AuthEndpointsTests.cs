using FluentAssertions;
using RealtimeChat.Application.DTOs;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace RealtimeChat.Tests.Integration;

public class AuthEndpointsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointsTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Login_Refresh_ReturnsTokens()
    {
        var email = $"user{Guid.NewGuid():N}@example.com";
        var password = "Password123!";

        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", new RegisterDto
        {
            Email = email,
            Password = password,
            DisplayName = "Test User"
        });

        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var registerPayload = await registerResponse.Content.ReadFromJsonAsync<AuthResultDto>();
        registerPayload.Should().NotBeNull();
        registerPayload!.AccessToken.Should().NotBeNullOrWhiteSpace();
        registerPayload.RefreshToken.Should().NotBeNullOrWhiteSpace();

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginDto
        {
            Email = email,
            Password = password
        });

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<AuthResultDto>();
        loginPayload.Should().NotBeNull();
        loginPayload!.AccessToken.Should().NotBeNullOrWhiteSpace();
        loginPayload.RefreshToken.Should().NotBeNullOrWhiteSpace();

        var refreshResponse = await _client.PostAsJsonAsync("/api/auth/refresh", new RefreshTokenRequestDto
        {
            RefreshToken = loginPayload.RefreshToken!
        });

        refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var refreshPayload = await refreshResponse.Content.ReadFromJsonAsync<AuthResultDto>();
        refreshPayload.Should().NotBeNull();
        refreshPayload!.AccessToken.Should().NotBeNullOrWhiteSpace();
        refreshPayload.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }
}
