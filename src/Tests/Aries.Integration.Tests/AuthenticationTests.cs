using System.Net;
using System.Text.Json;
using AriesContador.Core.Models.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Aries.Integration.Tests;

[TestFixture]
public class AuthenticationTests : IntegrationTest
{
    [Test]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        // Arrange
        var loginRequest = new Login
        {
            UserId = "kenneth",
            Password = "96321"
        };

        // Act
        var response = await _client.PostAsync("/Auth/login", GetJsonContent(loginRequest));
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<WebToken>(content, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.User.Should().NotBeNull();
        result.User.UserName.Should().Be("admin");
    }

    [Test]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var loginRequest = new Login
        {
            UserId = "invalid",
            Password = "invalid"
        };

        // Act
        var response = await _client.PostAsync("/Auth/login", GetJsonContent(loginRequest));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task ProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/User/GetAllUsers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task ProtectedEndpoint_WithValidToken_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await _client.GetAsync("/User/GetAllUsers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
} 