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

public class CreatePostCommandHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<CreatePostCommand, CreatePostCommandResult> _handler;

    public CreatePostCommandHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new CreatePostCommandHandler(_dbContext);
    }

    [Fact]
    public async Task PostShouldBeCreated()
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

        var command = new CreatePostCommand
        {
            AuthorUsername = username,
            Message = Guid.NewGuid().ToString()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.PostIsCreated.ShouldBeTrue();

    }

    [Fact]
    public async Task PostShouldNotBeCreatedWithInvalidUsername()
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

        var command = new CreatePostCommand
        {
            AuthorUsername = Guid.NewGuid().ToString(),
            Message = Guid.NewGuid().ToString()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.PostIsCreated.ShouldBeFalse();

    }

    [Fact]
    public async Task PostShouldNotBeCreatedIfOriginPostDoesNotExists()
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

        var command = new CreatePostCommand
        {
            AuthorUsername = Guid.NewGuid().ToString(),
            CommentTo = 9,
            Message = Guid.NewGuid().ToString()
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.PostIsCreated.ShouldBeFalse();

    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}