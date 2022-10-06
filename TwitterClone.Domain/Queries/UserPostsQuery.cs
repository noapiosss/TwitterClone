using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class UserPostsQuery : IRequest<UserPostsQueryResult>
{
    public string Username { get; init; }
}

public class UserPostsQueryResult
{
    public ICollection<Post> UserPosts { get; set; }
}

internal class UserPostsQueryHandler : IRequestHandler<UserPostsQuery, UserPostsQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public UserPostsQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UserPostsQueryResult> Handle(UserPostsQuery request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext.Users.AnyAsync(u => u.Username == request.Username)))
        {
            //probably need exception
            return null;
        }

        var userPosts = await _dbContext.Posts
            .Where(p => p.AuthorUsername == request.Username)
            .OrderByDescending(p => p.PostDate)
            .ToListAsync();

        return new UserPostsQueryResult
        {
            UserPosts = userPosts
        };
    }
}