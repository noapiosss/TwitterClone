using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Shouldly;

using TwitterClone.Contracts.Database;
using TwitterClone.Contracts.Http;
using TwitterClone.Domain.Database;

namespace TwitterClone.IntegrationTests;

public class FollowingControllerTests
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly HttpClient _client;
    private readonly User _user;

    public FollowingControllerTests()
    {
        _user = new User
        {
            Username = "integrationTestUser",
            Email = "integrationTestEmail",
            Password = "integrationTestPassword"
        };

        var app = new WebApplicationFactory<Program>();

        var scope = app.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<TwitterCloneDbContext>();
        _client = app.CreateClient();
    }

    [Fact]
    public async void FollowUserShouldChangeFollowStatus()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var rnd = new Random();

        var allUsernames = await _dbContext.Users.Select(u => u.Username).ToListAsync();
        var randomUsername = allUsernames[rnd.Next(allUsernames.Count)];
        var wasFollowed = await _dbContext.Followings.AnyAsync(f => f.FollowByUsername == _user.Username && f.FollowForUsername == randomUsername);

        var followUserRequest = new HttpRequestMessage(HttpMethod.Patch, "/api/follow");
        followUserRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        followUserRequest.Content = JsonContent.Create(new FollowUserRequest
        {
            FollowForUsername = randomUsername
        });
        followUserRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

        // Act
        using var response = await _client.SendAsync(followUserRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<FollowUserResponse>();
        result.FollowStatusIsChanged.ShouldBeTrue();

        var isFollowed = await _dbContext.Followings.AnyAsync(f => f.FollowByUsername == _user.Username && f.FollowForUsername == randomUsername);
        isFollowed.ShouldBe(!wasFollowed);
    }

    [Fact]
    public async void FollowUserShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange
        var rnd = new Random();

        var allUsernames = await _dbContext.Users.Select(u => u.Username).ToListAsync();
        var randomUsername = allUsernames[rnd.Next(allUsernames.Count)];

        var followUserRequest = new HttpRequestMessage(HttpMethod.Patch, "/api/follow");
        followUserRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        followUserRequest.Content = JsonContent.Create(new FollowUserRequest
        {
            FollowByUsername = _user.Username,
            FollowForUsername = randomUsername
        });
        followUserRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

        // Act
        using var response = await _client.SendAsync(followUserRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }

    [Fact]
    public async void FollowUserShouldBadRequestIfSelfFollowDetected()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var followUserRequest = new HttpRequestMessage(HttpMethod.Patch, "/api/follow");
        followUserRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        followUserRequest.Content = JsonContent.Create(new FollowUserRequest
        {
            FollowForUsername = _user.Username
        });
        followUserRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

        // Act
        using var response = await _client.SendAsync(followUserRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.NotAcceptable);
    }

    [Fact]
    public async void FollowUserShouldBadRequestIfUserNotExists()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var followUserRequest = new HttpRequestMessage(HttpMethod.Patch, "/api/follow");
        followUserRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        followUserRequest.Content = JsonContent.Create(new FollowUserRequest
        {
            FollowForUsername = Guid.NewGuid().ToString()
        });
        followUserRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

        // Act
        using var response = await _client.SendAsync(followUserRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.UserNotFound);
    }

    [Fact]
    public async void GetOwnFollowingsShouldReturnOwnFollowings()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        // Act
        using var response = await _client.GetAsync("/api/followings");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetFollowingsResponse>();
        var resultDb = await _dbContext.Followings
            .Where(f => f.FollowByUsername == _user.Username)
            .Select(f => f.FollowForUsername)
            .ToListAsync();

        result.Followings.ShouldBe(resultDb);
    }

    [Fact]
    public async void GetOwnFollowingsShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange

        // Act
        using var response = await _client.GetAsync("/api/followings");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }

    [Fact]
    public async void GetOwnFollowersShouldReturnOwnFollowers()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        // Act
        using var response = await _client.GetAsync("/api/followers");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetFollowersResponse>();
        var resultDb = await _dbContext.Followings
            .Where(f => f.FollowForUsername == _user.Username)
            .Select(f => f.FollowByUsername)
            .ToListAsync();

        result.Followers.ShouldBe(resultDb);
    }

    [Fact]
    public async void GetOwnFollowersShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange

        // Act
        using var response = await _client.GetAsync("/api/followers");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }

    [Fact]
    public async void GetUserFollowingsShouldReturnUserFollowings()
    {
        // Arrange
        var rnd = new Random();

        var allUsernames = await _dbContext.Users.Select(u => u.Username).ToListAsync();
        var randomUsername = allUsernames[rnd.Next(allUsernames.Count)];

        // Act
        using var response = await _client.GetAsync($"/api/{randomUsername}/followings");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetFollowingsResponse>();
        var resultDb = await _dbContext.Followings
            .Where(f => f.FollowByUsername == randomUsername)
            .Select(f => f.FollowForUsername)
            .ToListAsync();

        result.Followings.ShouldBe(resultDb);
    }

    [Fact]
    public async void GetUserFollowingsShouldReturnBadRequestIfUserNotExists()
    {
        // Arrange

        // Act
        using var response = await _client.GetAsync($"/api/{Guid.NewGuid().ToString()}/followings");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.UserNotFound);
    }

    [Fact]
    public async void GetUserFollowersShouldReturnUserFollowers()
    {
        // Arrange
        var rnd = new Random();

        var allUsernames = await _dbContext.Users.Select(u => u.Username).ToListAsync();
        var randomUsername = allUsernames[rnd.Next(allUsernames.Count)];

        // Act
        using var response = await _client.GetAsync($"/api/{randomUsername}/followers");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetFollowersResponse>();
        var resultDb = await _dbContext.Followings
            .Where(f => f.FollowForUsername == randomUsername)
            .Select(f => f.FollowByUsername)
            .ToListAsync();

        result.Followers.ShouldBe(resultDb);
    }

    [Fact]
    public async void GetUserFollowersShouldReturnBadRequestIfUserNotExists()
    {
        // Arrange

        // Act
        using var response = await _client.GetAsync($"/api/{Guid.NewGuid().ToString()}/followers");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.UserNotFound);
    }
}