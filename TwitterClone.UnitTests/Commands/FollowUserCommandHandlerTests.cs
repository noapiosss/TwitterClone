using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Shouldly;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using TwitterClone.Domain.Commands;
using TwitterClone.UnitTests.Helpers;

namespace TwitterClone.UnitTests.Queries;

public class FollowUserCommandHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<FollowUserCommand, FollowUserCommandResult> _handler;

    public FollowUserCommandHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new FollowUserCommandHandler(_dbContext);
    }

    [Fact]
    public async Task UserCouldFollowAnotherUser()
    {
        // Arrange
        var username1 = Guid.NewGuid().ToString();
        var user1 = new User
        {
            Username = username1,
            Email = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        var username2 = Guid.NewGuid().ToString();
        var user2 = new User
        {
            Username = username2,
            Email = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        await _dbContext.AddAsync(user1);
        await _dbContext.AddAsync(user2);
        await _dbContext.SaveChangesAsync();
        
        var command = new FollowUserCommand
        {
            FollowByUsername = username1,
            FollowForUsername = username2
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();        
        result.FollowStatusIsChanged.ShouldBeTrue();
    }

    [Fact]
    public async Task UserCouldNotFollowUnexistingUser()
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
        
        var command = new FollowUserCommand
        {
            FollowByUsername = username,
            FollowForUsername = Guid.NewGuid().ToString()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();        
        result.FollowStatusIsChanged.ShouldBeFalse();
    }

    [Fact]
    public async Task UnexistingUserCouldNotFollowUser()
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
        
        var command = new FollowUserCommand
        {
            FollowByUsername = Guid.NewGuid().ToString(),
            FollowForUsername = username
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();        
        result.FollowStatusIsChanged.ShouldBeFalse();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}