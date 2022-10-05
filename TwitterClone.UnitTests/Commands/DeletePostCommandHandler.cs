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

public class DeletePostCommandHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<DeletePostCommand, DeletePostCommandResult> _handler;

    public DeletePostCommandHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new DeletePostCommandHandler(_dbContext);
    }

    [Fact]
    public async Task PostShouldBeDeleted()
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

        var command = new DeletePostCommand
        {
            Username = username,
            PostId = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();        
        result.DeleteIsSuccessful.ShouldBeTrue();

    }

    [Fact]
    public async Task PostShouldNotBeDeletedIfInvalidPostId()
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

        var command = new DeletePostCommand
        {
            Username = username,
            PostId = 999
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();        
        result.DeleteIsSuccessful.ShouldBeFalse();

    }

    [Fact]
    public async Task PostShouldNotBeDeletedIfInvalidUsername()
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

        var command = new DeletePostCommand
        {
            Username = Guid.NewGuid().ToString(),
            PostId = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();        
        result.DeleteIsSuccessful.ShouldBeFalse();

    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}