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

public class HomePageQueryHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<HomePageQuery, HomePageQueryResult> _handler;

    public HomePageQueryHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new HomePageQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HomePageShouldBeSuccessfulIfInputIsCorrect()
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

        var countOfOwnPosts = rnd.Next(1, 10);

        for (int i = 0; i < countOfOwnPosts; ++i)
        {
            var ownPost = new Post
            {
                AuthorUsername = mainUsername,
                Message = Guid.NewGuid().ToString()
            };
            await _dbContext.AddAsync(ownPost);
        }

        await _dbContext.SaveChangesAsync();

        var countOfFollowings = rnd.Next(1, 10);
        var followingsUsernames = new List<string>();

        for (int i = 0; i < countOfFollowings; ++i)
        {
            followingsUsernames.Add(Guid.NewGuid().ToString());
        }

        foreach (var followingsUsername in followingsUsernames)
        {
            var user = new User
            {
                Username = followingsUsername,
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
            await _dbContext.AddAsync(user);
        }

        await _dbContext.SaveChangesAsync();

        int countOfFollowingsPosts = rnd.Next(1, 20);
        
        for (int i = 0; i < countOfFollowingsPosts; ++i)
        {
            var followingPost = new Post
            {
                AuthorUsername = followingsUsernames[rnd.Next(countOfFollowings)],
                Message = Guid.NewGuid().ToString()
            };
            await _dbContext.AddAsync(followingPost);
        }

        await _dbContext.SaveChangesAsync();

        foreach (var followingsUsername in followingsUsernames)
        {
            var following = new Following
            {
                FollowByUsername = mainUsername,
                FollowForUsername = followingsUsername
            };
            await _dbContext.AddAsync(following);
        }

        await _dbContext.SaveChangesAsync();


        var query = new HomePageQuery
        {
            Username = mainUsername
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.HomepagePosts.Count.ShouldBeEquivalentTo(countOfFollowingsPosts + countOfOwnPosts);
    }

    [Fact]
    public async Task HandlerShouldReturnNullIfUserUnexists()
    {
        // Arrange
        var query = new HomePageQuery
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