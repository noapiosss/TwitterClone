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

public class PostControllerTests
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly HttpClient _client;
    private readonly User _user;

    public PostControllerTests()
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
    public async void CreatePostShouldCreatePost()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);
        
        var createPostRequest = new CreatePostRequest
        {
            Message = Guid.NewGuid().ToString()
        };

        // Act
        using var response = await _client.PutAsJsonAsync("/api/posts", createPostRequest);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreatePostResponse>();
        result.PostIsCreated.ShouldBeTrue();
    }

    [Fact]
    public async void CreateCommentShouldReturnBadRequestIfOriginPostNotExists()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);
        
        var createPostRequest = new CreatePostRequest
        {
            CommentTo = -1,
            Message = Guid.NewGuid().ToString()
        };

        // Act
        using var response = await _client.PutAsJsonAsync("/api/posts", createPostRequest);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.OriginPostNotFound);
    }

    [Fact]
    public async void CreatePostShouldReturnBadRequestIfMessageIsInvalid()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);
        
        var createPostWithoutMessageRequest = new CreatePostRequest
        {
        };

        var createPostWithEmptyMessageRequest = new CreatePostRequest
        {
            Message = "   "
        };
        

        // Act
        using var responseWithoutMessage = await _client.PutAsJsonAsync("/api/posts", createPostWithoutMessageRequest);
        using var responseWithEmptyMessage = await _client.PutAsJsonAsync("/api/posts", createPostWithEmptyMessageRequest);
        
        // Assert
        responseWithoutMessage.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        responseWithEmptyMessage.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var resultWithoutMessage = await responseWithoutMessage.Content.ReadFromJsonAsync<ErrorResponse>();
        var resultWithEmptyMessage = await responseWithEmptyMessage.Content.ReadFromJsonAsync<ErrorResponse>();
        resultWithoutMessage.Code.ShouldBe(ErrorCode.InvalidMessage);
        resultWithEmptyMessage.Code.ShouldBe(ErrorCode.InvalidMessage);
    }

    [Fact]
    public async void CreatePostShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange
        var createPostWithoutMessageRequest = new CreatePostRequest
        {
            AuthorUsername = Guid.NewGuid().ToString(),
            Message = Guid.NewGuid().ToString()
        };
        

        // Act
        using var response = await _client.PutAsJsonAsync("/api/posts", createPostWithoutMessageRequest);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }

    [Fact]
    public async void GetPostShouldReturnPost()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);
        
        var message = Guid.NewGuid().ToString();
        var createPostRequest = new CreatePostRequest
        {
            Message = message
        };        
        var createPostResult = await (await _client.PutAsJsonAsync("/api/posts", createPostRequest))
            .Content.ReadFromJsonAsync<CreatePostResponse>();
        
        // Act
        using var response = await _client.GetAsync($"/api/posts/{createPostResult.Post.PostId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<GetPostResponse>();
        var resultDb = await _dbContext.Posts
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.PostId == createPostResult.Post.PostId);
        var commentsDb = await _dbContext.Posts
            .Where(p => p.CommentTo == createPostResult.Post.PostId)
            .OrderByDescending(p => p.PostId)
            .ToListAsync();

        result.Post.PostId.ShouldBe(resultDb.PostId);
        result.Post.Author.ShouldBe(resultDb.Author);
        result.Post.PostDate.ShouldBe(resultDb.PostDate);
        result.Post.Message.ShouldBe(resultDb.Message);
        result.LikedByUsername.ShouldBe(resultDb.Likes.Select(l => l.LikedByUsername).ToList());
        result.Comments.ShouldBe(commentsDb);
    }

    [Fact]
    public async void GetPostShouldReturnBadRequestIfPostNotExists()
    {
        // Arrange
        
        // Act
        using var response = await _client.GetAsync($"/api/posts/{-1}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.PostNotFound);
    }

    [Fact]
    public async void DeletePostShouldDeletePost()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);
        
        var createPostRequest = new CreatePostRequest
        {
            Message = Guid.NewGuid().ToString()
        };        
        await _client.PutAsJsonAsync("/api/posts", createPostRequest);

        var postId = (await _dbContext.Posts.FirstAsync(p => p.AuthorUsername == _user.Username)).PostId;
        var deletePostRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/posts");
        deletePostRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        deletePostRequest.Content = JsonContent.Create(new DeletePostRequest
            {
                PostId = postId
            });
        deletePostRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
        
        // Act
        using var response = await _client.SendAsync(deletePostRequest);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<DeletePostResponse>();
        result.DeleteIsSuccessful.ShouldBeTrue();
        (await _dbContext.Posts.AnyAsync(p => p.PostId == postId)).ShouldBeFalse();
    }

    [Fact]
    public async void DeletePostShouldReturnBadRequestIfUnauthorized()
    {
        // Arrange
        var postId = (await _dbContext.Posts.FirstAsync()).PostId;
        var deletePostRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/posts");
        deletePostRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        deletePostRequest.Content = JsonContent.Create(new DeletePostRequest
            {
                PostId = postId
            });
        deletePostRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
        
        // Act
        using var response = await _client.SendAsync(deletePostRequest);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.Unauthorized);
    }

    [Fact]
    public async void DeletePostShouldReturnBadRequestIfPostNotExists()
    {
        // Arrange
        var signInRequest = new SignInRequest
        {
            Username = _user.Username,
            Password = _user.Password
        };
        await _client.PostAsJsonAsync("/sign-in", signInRequest);

        var deletePostRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/posts");
        deletePostRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        deletePostRequest.Content = JsonContent.Create(new DeletePostRequest
            {
                PostId = -1
            });
        deletePostRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
        
        // Act
        using var response = await _client.SendAsync(deletePostRequest);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        var result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result.Code.ShouldBe(ErrorCode.PostNotFound);
    }
}