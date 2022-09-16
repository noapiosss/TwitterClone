using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class UserQuery : IRequest<UserQueryResult>
{
    public string Username { get; init; }
}

public class UserQueryResult
{
    public List<Post> UserPosts { get; set; }
}

public class UserQueryHandler : IRequestHandler<UserQuery, UserQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public UserQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserQueryResult> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        var userPosts = _dbContext.Posts.ToListAsync().Result.ToList().Where(p => p.AuthorUsername == request.Username).ToList();

        return new UserQueryResult
        {
            UserPosts = userPosts
        };
    }
}