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
        if (!(await _dbContext.Users.AnyAsync(u => u.Username == request.Username)))
        {
            //probably should be exception
            return null;
        }

        var likedPosts = await _dbContext.Likes
            .Where(l => l.LikedByUsername == request.Username)
            .Include(l => l.LikedPost)
            .ToListAsync();
            
        return new FavoritesQueryResult
        {
            FavoritesPosts = likedPosts.Select(l => l.LikedPost).OrderByDescending(p => p.PostId).ToList()
        };
    }
}