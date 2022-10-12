using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Shouldly;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using TwitterClone.Domain.Helpers;
using TwitterClone.Domain.Queries;
using TwitterClone.UnitTests.Helpers;

namespace TwitterClone.UnitTests.Queries;

public class FavoritesQueryHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<FavoritesQuery, FavoritesQueryResult> _handler;

    public FavoritesQueryHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new FavoritesQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HandlerShouldReturnLikedPosts()
    {
        // Arrange
        var mainUsername = Guid.NewGuid().ToString();

        var mainUser = new User
        {
            Username = mainUsername,
            Email = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        await _dbContext.AddAsync(mainUser);
        await _dbContext.SaveChangesAsync();

        var rnd = new Random();

        var anotherUsernames = new List<string>();
        var anotherUsersCount = rnd.Next(1, 10);

        for (int i = 0; i < anotherUsersCount; ++i)
        {
            var username = Guid.NewGuid().ToString();
            var user = new User
            {
                Username = username,
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
            anotherUsernames.Add(username);
            await _dbContext.AddAsync(user);
        }

        var anotherUsersPostsCount = rnd.Next(1, 20);

        for (int i = 0; i < anotherUsersCount; ++i)
        {
            var post = new Post
            {
                AuthorUsername = anotherUsernames[rnd.Next(1, anotherUsersCount) - 1],
                Message = Guid.NewGuid().ToString()
            };
            await _dbContext.AddAsync(post);
        }

        await _dbContext.SaveChangesAsync();

        var countOfLikedPosts = rnd.Next(1, anotherUsersCount);
        for (int i = 1; i <= countOfLikedPosts; ++i)
        {
            var like = new Like
            {
                LikedByUsername = mainUsername,
                LikedPostId = i
            };
            await _dbContext.AddAsync(like);
        }

        await _dbContext.SaveChangesAsync();

        var query = new FavoritesQuery
        {
            Username = mainUsername
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.FavoritesPosts.Count.ShouldBeEquivalentTo(countOfLikedPosts);
    }

    [Fact]
    public async Task HandlerShouldReturnNullIfUserUnexists()
    {
        // Arrange
        var query = new FavoritesQuery
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