using System;
using System.Linq;
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

public class UserLikesQueryHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<UserLikesQuery, UserLikesQueryResult> _handler;

    public UserLikesQueryHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new UserLikesQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HandlerSouldReutrnLikesListIfInputIsCorrect()
    {
        // Arrange
        var likerUsername = Guid.NewGuid().ToString();
        var likerUser = new User
        {
            Username = likerUsername,
            Email = Guid.NewGuid().ToString(),
            Password = Guid.NewGuid().ToString()
        };

        await _dbContext.AddAsync(likerUser);
        await _dbContext.SaveChangesAsync();

        var rnd = new Random();
        
        var anotherUsersCount = rnd.Next(1, 10);
        for (int i = 0; i < anotherUsersCount; ++i)
        {
            var anotherUser = new User
            {
                Username = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
            await _dbContext.AddAsync(anotherUser);
        }

        await _dbContext.SaveChangesAsync();

        var anotherUsers = _dbContext.Users.Where(u => u.Username != likerUsername).ToList();
        var anotherPostsCount = rnd.Next(1, 20);
        for (int i = 0; i < anotherPostsCount; ++i)
        {
            var post = new Post
            {
                AuthorUsername = anotherUsers[rnd.Next(anotherUsers.Count)].Username,
                Message = Guid.NewGuid().ToString()
            };
            await _dbContext.AddAsync(post);
        }

        await _dbContext.SaveChangesAsync();
        
        var likedPostsCount = rnd.Next(0, anotherPostsCount);
        for (int i = 1; i <= likedPostsCount; ++i)
        {
            var like = new Like
            {
                LikedByUsername = likerUsername,
                LikedPostId = i
            };
            await _dbContext.AddAsync(like);
        }
        
        await _dbContext.SaveChangesAsync();

        var query = new UserLikesQuery
        {
            Username = likerUsername            
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.PostIdsThatUserLike.Count.ShouldBeEquivalentTo(likedPostsCount);        
    }

    [Fact]
    public async Task HandlerShouldReturnNullIfUserUnexists()
    {
        // Arrange
        var query = new UserLikesQuery
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