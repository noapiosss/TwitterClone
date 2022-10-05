using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class FollowersQuery : IRequest<FollowersQueryResult>
{
    public string Username { get; init; }
}

public class FollowersQueryResult
{
    public ICollection<string> Followers { get; set; }
}

internal class FollowingQueryHandler : IRequestHandler<FollowersQuery, FollowersQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public FollowingQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<FollowersQueryResult> Handle(FollowersQuery request, CancellationToken cancellationToken)
    {
        var followers = _dbContext.Followings.Where(f => f.FollowForUsername == request.Username).ToList();
        var followersUsername = new List<string>();

        foreach (var follower in followers)
        {
            followersUsername.Add(follower.FollowByUsername);
        }        

        return new FollowersQueryResult
        {
            Followers = followersUsername
        };
    }
}