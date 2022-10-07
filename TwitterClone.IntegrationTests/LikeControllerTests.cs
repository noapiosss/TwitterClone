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

public class LikeControllerTests
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly HttpClient _client;
    private readonly User _user;

    public LikeControllerTests()
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
    public async void LikePostShouldChangeLikeStatus()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var rnd = new Random();

        var allPostIds = await _dbContext.Posts.Select(p => p.PostId).ToListAsync();
        var randomPostId = allPostIds[rnd.Next(allPostIds.Count)];
        var wasLiked = await _dbContext.Likes.AnyAsync(l => l.LikedPostId == randomPostId && l.LikedByUsername == _user.Username);
        
        var likePostRequest = new HttpRequestMessage(HttpMethod.Patch, "/api/likes");
        likePostRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        likePostRequest.Content = JsonContent.Create(new LikePostRequest
            {
                LikedPostId = randomPostId
            });
        likePostRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");        

        // Act
        using var response = await _client.SendAsync(likePostRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<LikePostResponse>();
        result.LikeStatusIsChanged.ShouldBeTrue();

        var isLiked = await _dbContext.Likes.AnyAsync(l => l.LikedPostId == randomPostId && l.LikedByUsername == _user.Username);
        isLiked.ShouldBe(!wasLiked);
    }

    [Fact]
    public async void LikePostShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange
        var likePostRequest = new HttpRequestMessage(HttpMethod.Patch, "/api/likes");
        likePostRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        likePostRequest.Content = JsonContent.Create(new LikePostRequest
            {
                LikedPostId = 1,
                LikedByUsername = _user.Username
            });
        likePostRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

        // Act
        using var response = await _client.SendAsync(likePostRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }

    [Fact]
    public async void LikePostShouldReturnBadRequestIfPostNotExists()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);
        
        var likePostRequest = new HttpRequestMessage(HttpMethod.Patch, "/api/likes");
        likePostRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        likePostRequest.Content = JsonContent.Create(new LikePostRequest
            {
                LikedPostId = -1,
                LikedByUsername = _user.Username
            });
        likePostRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

        // Act
        using var response = await _client.SendAsync(likePostRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.PostNotFound);
    }

    [Fact]
    public async void GetPostLikesShouldReturnUsernamesThatLikePost()
    {
        // Arrange
        var rnd = new Random();

        var allPostIds = await _dbContext.Posts.Select(p => p.PostId).ToListAsync();
        var randomPostId = allPostIds[rnd.Next(allPostIds.Count)];

        // Act
        using var response = await _client.GetAsync($"/api/likes/{randomPostId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetLikesResponse>();
        var resultDb = await _dbContext.Likes
            .Where(l => l.LikedPostId == randomPostId)
            .Select(l => l.LikedByUsername)
            .ToListAsync();

        result.UsersThatLikePost.ShouldBe(resultDb);
    }

    [Fact]
    public async void GetPostLikesShouldReturnBadRequestIfPostNotExists()
    {
        // Arrange

        // Act
        using var response = await _client.GetAsync($"/api/likes/{-1}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.PostNotFound);
    }

    [Fact]
    public async void GetUserLikesShouldReturnPostIdsThatUserLike()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var rnd = new Random();
        var randomLikeActionCount = rnd.Next(1,10);
        var allPostIds = await _dbContext.Posts.Select(p => p.PostId).ToListAsync();

        for (int i = 0; i < randomLikeActionCount; ++i)
        {
            var likePostRequest = new HttpRequestMessage(HttpMethod.Patch, "/api/likes");
            likePostRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            likePostRequest.Content = JsonContent.Create(new LikePostRequest
                {
                    LikedByUsername = _user.Username,
                    LikedPostId = allPostIds[rnd.Next(allPostIds.Count)]
                });
            likePostRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
            await _client.SendAsync(likePostRequest);
        }

        // Act
        using var response = await _client.GetAsync($"/api/likes");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetPostsThatUserLikeResponse>();
        var resultBd = await _dbContext.Likes
            .Where(l => l.LikedByUsername == _user.Username)
            .Select(l => l.LikedPostId)
            .ToListAsync();

        result.PostIdsThatUserLike.ShouldBe(resultBd);
    }

    [Fact]
    public async void GetUserLikesShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange
         
        // Act
        using var response = await _client.GetAsync($"/api/likes");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }
}