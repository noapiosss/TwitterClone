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

public class UserQueryHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<UserQuery, UserQueryResult> _handler;

    public UserQueryHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new UserQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HandleShouldReturnUser()
    {
        // Arrange
        var user = new User
        {
            Username = Guid.NewGuid().ToString(),
            Email = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };
        user.AuthoredPosts.Add(new Post
        {
            PostDate = new DateTime(2005, 01, 02),
            Message = Guid.NewGuid().ToString()
        });

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        var query = new UserQuery
        {
            Username = user.Username
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.User.ShouldNotBeNull();
        result.User.Username.ShouldBe(user.Username);
        result.User.Email.ShouldBe(user.Email);
        result.User.Password.ShouldBe(user.Password.GetHashCode().ToString());

    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}