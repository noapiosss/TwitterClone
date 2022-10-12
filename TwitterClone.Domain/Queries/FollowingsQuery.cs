using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

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

internal class FollowingsQueryHandler : IRequestHandler<FollowingsQuery, FollowingsQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public FollowingsQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<FollowingsQueryResult> Handle(FollowingsQuery request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Users.AnyAsync(u => u.Username == request.Username, cancellationToken))
        {
            return null;
        }

        var followings = await _dbContext.Followings
            .Where(f => f.FollowByUsername == request.Username)
            .Select(f => f.FollowForUsername)
            .ToListAsync(cancellationToken);

        return new FollowingsQueryResult
        {
            Followings = followings
        };
    }
}