using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Shouldly;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using TwitterClone.Domain.Queries;
using TwitterClone.UnitTests.Helpers;

namespace TwitterClone.UnitTests.Queries;

public class UserPostsQueryHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<UserPostsQuery, UserPostsQueryResult> _handler;

    public UserPostsQueryHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new UserPostsQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HandleShouldReturnUserPosts()
    {
        // Arrange
        var username = Guid.NewGuid().ToString();
        var user = new User
        {
            Username = username,
            Email = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var postsCount = 10;

        for (int i = 0; i < postsCount; ++i)
        {
            var userPost = new Post
            {
                AuthorUsername  = username,
                PostDate = new DateTime(2005, 5, 5, 5, 5, 5),
                Message = Guid.NewGuid().ToString()
            };
            await _dbContext.AddAsync(userPost);
            await _dbContext.SaveChangesAsync();
        }

        var query = new UserPostsQuery
        {
            Username = user.Username
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.UserPosts.Count.ShouldBeEquivalentTo(postsCount); //probably need to check something else
    }

    [Fact]
    public async Task HandleShouldReturnNullIfUserNotExists()
    {
        // Arrange
        var query = new UserPostsQuery
        {
            Username = Guid.NewGuid().ToString()
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldBeNull();

    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}