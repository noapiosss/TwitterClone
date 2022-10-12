using System;
using System.Collections.Generic;
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

public class UserControllerTests
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly HttpClient _client;
    private readonly User _user;

    public UserControllerTests()
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
    public async void CreateUserShouldCreateUser()
    {
        // Arrange
        var username = Guid.NewGuid().ToString();
        var email = Guid.NewGuid().ToString();
        var password = Guid.NewGuid().ToString();

        var request = new CreateUserRequest
        {
            Username = username,
            Email = email,
            Password = password
        };

        // Act
        using var response = await _client.PutAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
        result.UsernameIsAlreadyInUse.ShouldBeFalse();
        result.EmailIsAlreadyInUse.ShouldBeFalse();
        result.IsRegistrationSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async void CreateUserShouldNotCreateUserIfUsernameAlreadyInUser()
    {
        // Arrange
        var username = _user.Username;
        var email = Guid.NewGuid().ToString();
        var password = Guid.NewGuid().ToString();

        var request = new CreateUserRequest
        {
            Username = username,
            Email = email,
            Password = password
        };

        // Act
        using var response = await _client.PutAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.UsernameIsAlreadyInUse);
    }

    [Fact]
    public async void CreateUserShouldNotCreateUserIfEmailAlreadyInUser()
    {
        // Arrange
        var username = Guid.NewGuid().ToString();
        var email = _user.Email;
        var password = Guid.NewGuid().ToString();

        var request = new CreateUserRequest
        {
            Username = username,
            Email = email,
            Password = password
        };

        // Act
        using var response = await _client.PutAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.EmailIsAlreadyInUse);
    }

    [Fact]
    public async void GetUserPostsShouldReturnUserPosts()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var createPostRequest = new Post
        {
            Message = Guid.NewGuid().ToString()
        };
        await _client.PutAsJsonAsync("/api/posts", createPostRequest);

        // Act
        using var response = await _client.GetAsync($"/api/users/{_user.Username}/posts");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = (await response.Content.ReadFromJsonAsync<GetUserPostsResponse>()).UserPosts.ToList();
        var resultDb = await _dbContext.Posts
            .Where(p => p.AuthorUsername == _user.Username)
            .OrderByDescending(p => p.PostDate)
            .ToListAsync();
        result.Count.ShouldBeEquivalentTo(resultDb.Count());
        for (int i = 0; i < resultDb.Count; ++i)
        {
            result[i].PostId.ShouldBeEquivalentTo(resultDb[i].PostId);
        }
    }

    [Fact]
    public async void GetUserPostsShouldReturnBadRequestIfUserNotExists()
    {
        // Arrange

        // Act
        using var response = await _client.GetAsync($"/api/users/{Guid.NewGuid().ToString()}/posts");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.UserNotFound);
    }

    [Fact]
    public async void GetHomePagePostsShouldReturnPosts()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var getHomePagePostsRequest = new GetHomePagePostsRequest();

        // Act
        using var response = await _client.GetAsync($"/api/users/homepage");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = (await response.Content.ReadFromJsonAsync<GetHomePagePostsResponse>()).HomepagePosts.ToList();

        var userFollowings = await _dbContext.Followings
            .Where(f => f.FollowByUsername == _user.Username)
            .Include(f => f.FollowForUser.Posts)
            .ToListAsync();

        var userOwnPosts = await _dbContext.Posts
            .Where(p => p.AuthorUsername == _user.Username)
            .ToListAsync();

        var resultDb = new List<Post>();
        resultDb.AddRange(userOwnPosts);
        resultDb.AddRange(userFollowings.SelectMany(f => f.FollowForUser.Posts).ToList());
        resultDb = resultDb.OrderByDescending(p => p.PostId).ToList();

        result.Count.ShouldBeEquivalentTo(resultDb.Count);
        for (int i = 0; i < result.Count; ++i)
        {
            result[i].PostId.ShouldBeEquivalentTo(resultDb[i].PostId);
        }
    }

    [Fact]
    public async void GetHomePagePostsShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange

        // Act
        using var response = await _client.GetAsync("/api/users/homepage");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }

    [Fact]
    public async void GetFavoritesPageShouldReturnLikedPosts()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var rnd = new Random();
        var randomLikeActionCount = rnd.Next(1, 10);
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
        using var response = await _client.GetAsync("/api/users/favorites");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = (await response.Content.ReadFromJsonAsync<GetFavoritesPostsResponse>()).FavoritesPosts.ToList();
        var resultDb = (await _dbContext.Likes
            .Where(l => l.LikedByUsername == _user.Username)
            .Include(l => l.LikedPost)
            .ToListAsync())
                .Select(l => l.LikedPost)
                .OrderByDescending(p => p.PostId)
                .ToList();

        result.Count.ShouldBeEquivalentTo(resultDb.Count);
        for (int i = 0; i < result.Count; ++i)
        {
            result[i].PostId.ShouldBeEquivalentTo(resultDb[i].PostId);
        }
    }

    [Fact]
    public async void GetFavoritesPageShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange        

        // Act
        using var response = await _client.GetAsync("/api/users/favorites");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }

    [Fact]
    public async void GetSessionUsernameShouldReturnUsername()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        // Act
        var response = await _client.GetAsync("/api/users/username");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldBeEquivalentTo(_user.Username);
    }

    [Fact]
    public async void GetSessionUsernameShouldReturnNullIfUnauthorized()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/api/users/username");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}