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

public class FollowingsQueryHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<FollowingsQuery, FollowingsQueryResult> _handler;

    public FollowingsQueryHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new FollowingsQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HandlerShouldReturnFollowingsUsernames()
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

        var usernamesThatFollowed = new List<string>();
        var countOfFollowings = rnd.Next(1, 10);

        for (int i = 0; i < countOfFollowings; ++i)
        {
            var username = Guid.NewGuid().ToString();
            var user = new User
            {
                Username = username,
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
            usernamesThatFollowed.Add(username);
            await _dbContext.AddAsync(user);
        }

        await _dbContext.SaveChangesAsync();

        for (int i = 0; i < countOfFollowings; ++i)
        {
            var following = new Following
            {
                FollowByUsername = mainUsername,
                FollowForUsername = usernamesThatFollowed[i]
            };
            await _dbContext.AddAsync(following);
        }

        await _dbContext.SaveChangesAsync();

        var query = new FollowingsQuery
        {
            Username = mainUsername
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Followings.Count.ShouldBeEquivalentTo(countOfFollowings);
    }

    [Fact]
    public async Task HandlerShouldReturnNullIfUserUnexists()
    {
        // Arrange
        var query = new FollowingsQuery
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