using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
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

public class LikeQueryHandler : IRequestHandler<LikeQuery, LikeQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public LikeQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<LikeQueryResult> Handle(LikeQuery request, CancellationToken cancellationToken)
    {
        var postLikes = _dbContext.Likes.Where(l => l.LikedPostId == request.PostId).ToList();
        
        List<string> usersThatLikePost = new List<string>();
        
        foreach(var like in postLikes)
        {
            var user = _dbContext.Users.First(u => u.Username == like.LikedByUsername);
            usersThatLikePost.Add(user.Username);
        }


        return new LikeQueryResult
        {
            UsersThatLikePost = usersThatLikePost
        };
    }
}