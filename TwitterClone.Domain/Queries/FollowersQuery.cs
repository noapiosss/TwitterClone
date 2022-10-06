using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

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

internal class FollowersQueryHandler : IRequestHandler<FollowersQuery, FollowersQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public FollowersQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<FollowersQueryResult> Handle(FollowersQuery request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext.Users.AnyAsync(u => u.Username == request.Username)))
        {
            //probably should be exception
            return null;
        }

        var followers = await _dbContext.Followings
            .Where(f => f.FollowForUsername == request.Username)
            .ToListAsync();

        return new FollowersQueryResult
        {
            Followers = followers.Select(f => f.FollowByUsername).ToList()
        };
    }
}