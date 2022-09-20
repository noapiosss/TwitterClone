using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class FollowingsQuery : IRequest<FollowingsQueryResult>
{
    public string Username { get; init; }
}

public class FollowingsQueryResult
{
    public ICollection<string> Followings { get; set; }
}

public class FollowingsQueryHandler : IRequestHandler<FollowingsQuery, FollowingsQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public FollowingsQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<FollowingsQueryResult> Handle(FollowingsQuery request, CancellationToken cancellationToken)
    {
        var followings = _dbContext.Followings.Where(f => f.FollowByUsername == request.Username).ToList();
        var followingsUsername = new List<string>();

        foreach (var following in followings)
        {
            followingsUsername.Add(following.FollowForUsername);
        }        

        return new FollowingsQueryResult
        {
            Followings = followingsUsername
        };
    }
}