using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class LikeQuery : IRequest<LikeQueryResult>
{
    public int PostId { get; init; }
}

public class LikeQueryResult
{
    public ICollection<string> UsersThatLikePost { get; set; }
}

internal class LikeQueryHandler : IRequestHandler<LikeQuery, LikeQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public LikeQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<LikeQueryResult> Handle(LikeQuery request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext.Posts.AnyAsync(p => p.PostId == request.PostId)))
        {
            return null;
        }

        var usersThatLikePost = await _dbContext.Likes
            .Where(l => l.LikedPostId == request.PostId)
            .Select(l => l.LikedByUsername)
            .ToListAsync();

        return new LikeQueryResult
        {
            UsersThatLikePost = usersThatLikePost
        };
    }
}