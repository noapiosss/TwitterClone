using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class UserLikesQuery : IRequest<UserLikesQueryResult>
{
    public string Username { get; init; }
}

public class UserLikesQueryResult
{
    public ICollection<int> PostIdsThatUserLike { get; set; }
}

internal class UserLikesQueryHandler : IRequestHandler<UserLikesQuery, UserLikesQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public UserLikesQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UserLikesQueryResult> Handle(UserLikesQuery request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Users.AnyAsync(u => u.Username == request.Username))
        {
            return null;
        }

        var postIdsThatUserLike = await _dbContext.Likes
            .Where(l => l.LikedByUsername == request.Username)
            .Select(l => l.LikedPostId)
            .ToListAsync();

        return new UserLikesQueryResult
        {
            PostIdsThatUserLike = postIdsThatUserLike
        };
    }
}