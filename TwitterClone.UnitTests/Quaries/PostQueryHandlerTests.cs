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

public class PostQueryHandlerTest : IDisposable
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<PostQuery, PostQueryResult> _handler;

    public PostQueryHandlerTest()
    {
        _dbContext = DbContextHelper.CreateTestDb();
        _handler = new PostQueryHandler(_dbContext);
    }

    [Fact]
    public async Task HandlerShouldReturnPostCommentsAndLikes()
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

        var postMessage = Guid.NewGuid().ToString();
        var post = new Post
        {
            AuthorUsername = username,
            Message = postMessage
        };

        await _dbContext.AddAsync(post);
        await _dbContext.SaveChangesAsync();

        var like = new Like
        {
            LikedByUsername = username,
            LikedPostId = 1
        };

        await _dbContext.AddAsync(like);
        await _dbContext.SaveChangesAsync();

        var rnd = new Random();

        var commentsCount = rnd.Next(1, 10);

        for (int i = 0; i < commentsCount; ++i)
        {
            var comment = new Post
            {
                AuthorUsername = username,
                CommentTo = 1,
                Message = Guid.NewGuid().ToString()
            };

            await _dbContext.AddAsync(comment);
        }

        await _dbContext.SaveChangesAsync();

        var query = new PostQuery
        {
            PostId = 1
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Post.AuthorUsername.ShouldBeEquivalentTo(username);
        result.Post.Message.ShouldBeEquivalentTo(postMessage);
        result.LikedByUsername.Count.ShouldBeEquivalentTo(1);
        result.Comments.Count.ShouldBeEquivalentTo(commentsCount);
    }

    [Fact]
    public async Task HandlerShouldReturnNullIfPostUnexists()
    {
        // Arrange
        var query = new PostQuery
        {
            PostId = 1
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