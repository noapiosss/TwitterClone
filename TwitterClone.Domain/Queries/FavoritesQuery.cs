using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class FavoritesQuery : IRequest<FavoritesQueryResult>
{
    public string Username { get; init; }
}

public class FavoritesQueryResult
{
    public ICollection<Post> FavoritesPosts { get; set; }
}

internal class FavoritesQueryHandler : IRequestHandler<FavoritesQuery, FavoritesQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public FavoritesQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<FavoritesQueryResult> Handle(FavoritesQuery request, CancellationToken cancellationToken)
    {
        var likedPosts = _dbContext.Likes
            .Where(l => l.LikedByUsername == request.Username)
            .Include(l => l.LikedPost)
            .ToList();
        
        List<Post> favoritesPosts = new List<Post>();
        
        foreach(var post in likedPosts)
        {
            favoritesPosts.Add(post.LikedPost);
        }


        return new FavoritesQueryResult
        {
            FavoritesPosts = favoritesPosts.OrderByDescending(f => f.PostId).ToList()
        };
    }
}