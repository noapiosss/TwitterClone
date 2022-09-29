using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
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

public class UserLikesQueryHandler : IRequestHandler<UserLikesQuery, UserLikesQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public UserLikesQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UserLikesQueryResult> Handle(UserLikesQuery request, CancellationToken cancellationToken)
    {
        var likes = _dbContext.Likes.Where(l => l.LikedByUsername == request.Username).ToList();
        
        List<int> postIdsThatUserLike = new List<int>();
        
        foreach(var like in likes)
        {
            postIdsThatUserLike.Add(like.LikedPostId);
        }

        return new UserLikesQueryResult
        {
            PostIdsThatUserLike = postIdsThatUserLike
        };
    }
}