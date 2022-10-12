using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Shouldly;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Commands;
using TwitterClone.Domain.Database;
using TwitterClone.UnitTests.Helpers;

namespace TwitterClone.UnitTests.Queries;

public class LikePostCommandHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<LikePostCommand, LikePostCommandResult> _handler;

    public LikePostCommandHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new LikePostCommandHandler(_dbContext);
    }

    [Fact]
    public async Task UserCouldLikeAnyExistsPost()
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

        var post = new Post
        {
            AuthorUsername = username,
            Message = Guid.NewGuid().ToString()
        };

        await _dbContext.AddAsync(post);
        await _dbContext.SaveChangesAsync();

        var command = new LikePostCommand
        {
            LikedPostId = 1,
            LikedByUsername = username
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.LikeStatusIsChanged.ShouldBeTrue();
    }

    [Fact]
    public async Task UserCouldNotLikeUnxistingPost()
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

        var command = new LikePostCommand
        {
            LikedPostId = 1,
            LikedByUsername = username
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.LikeStatusIsChanged.ShouldBeFalse();
    }

    [Fact]
    public async Task UnexistingUserCouldNotLikeExistsPost()
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

        var post = new Post
        {
            AuthorUsername = username,
            Message = Guid.NewGuid().ToString()
        };

        await _dbContext.AddAsync(post);
        await _dbContext.SaveChangesAsync();

        var command = new LikePostCommand
        {
            LikedPostId = 1,
            LikedByUsername = Guid.NewGuid().ToString()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.LikeStatusIsChanged.ShouldBeFalse();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}