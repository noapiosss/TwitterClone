using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

using Moq;

using Shouldly;

using TwitterClone.Contracts.Database;
using TwitterClone.Contracts.Http;

namespace TwitterClone.IntegrationTests;

public class TwitterCloneApiTests
{
    private readonly HttpClient _client;

    private readonly User _user;

    public TwitterCloneApiTests()
    {
        _user = new User
        {
            Username = "integrationTestUser",
            Email = "integrationTestEmail",
            Password = "integrationTestPassword"
        };

        var app = new WebApplicationFactory<Program>();
        _client = app.CreateClient();
    }

    [Fact]
    public async void SignInShouldCreateSession()
    {
        // Arrange
        var request = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };

        // Act
        using var response = await _client.PostAsJsonAsync("/sign-in", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async void SignInShouldReturnBadRequestIfWrongPassword()
    {
        // Arrange
        var request = new SignInRequest
        {
            Username = _user.Username,
            Password = Guid.NewGuid().ToString()
        };

        // Act
        using var response = await _client.PostAsJsonAsync("/sign-in", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.WrongPassword);
    }

    [Fact]
    public async void SignInShouldReturnBadRequestIfUserNotExists()
    {
        // Arrange
        var request = new SignInRequest
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        // Act
        using var response = await _client.PostAsJsonAsync("/sign-in", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.UserNotFound);
    }

    [Fact]
    public async void SignOutShouldDeleteCoockie()
    {
        // Arrange
        var request = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", request);

        // Act
        await _client.GetAsync("/sign-out");
        var response = await _client.GetAsync("/api/users/username");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}