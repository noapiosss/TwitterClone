using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class UserQuery : IRequest<UserQueryResult>
{
    public string Username { get; init; }
}

public class UserQueryResult
{
    public ICollection<Post> UserPosts { get; set; }
}

internal class UserQueryHandler : IRequestHandler<UserQuery, UserQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public UserQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UserQueryResult> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        var userPosts = _dbContext.Posts
            .Where(p => p.AuthorUsername == request.Username)
            .OrderByDescending(p => p.PostDate)
            .ToList();

        return new UserQueryResult
        {
            UserPosts = userPosts
        };
    }
}