using System;
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

public class SignInQueryHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<SignInQuery, SignInQueryResult> _handler;

    public SignInQueryHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new SignInQueryHandler(_dbContext);
    }

    [Fact]
    public async Task SignInShouldBeSuccessfulIfInputIsCorrect()
    {
        // Arrange
        var username = Guid.NewGuid().ToString();
        var password = Guid.NewGuid().ToString();

        var user = new User
        {
            Username = username,
            Email = Guid.NewGuid().ToString(),
            Password = PasswordHelper.GetPasswordHash(password)
        };

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        var query = new SignInQuery
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsAuthenticated.ShouldBeTrue();
    }

    [Fact]
    public async Task SignInShouldNotBeSuccessfulIfUserUnexists()
    {
        // Arrange
        var query = new SignInQuery
        {
            Username = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsAuthenticated.ShouldBeFalse();
        result.UserIsFound.ShouldBeFalse();
    }

    [Fact]
    public async Task SignInShouldNotBeSuccessfulIfPasswordIsWrong()
    {
        // Arrange
        var username = Guid.NewGuid().ToString();
        var password = Guid.NewGuid().ToString();

        var user = new User
        {
            Username = username,
            Email = Guid.NewGuid().ToString(),
            Password = PasswordHelper.GetPasswordHash(password)
        };

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        var query = new SignInQuery
        {
            Username = username,
            Password = Guid.NewGuid().ToString()
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsAuthenticated.ShouldBeFalse();
        result.UserIsFound.ShouldBeTrue();
        result.PasswordIsCorrect.ShouldBeFalse();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}