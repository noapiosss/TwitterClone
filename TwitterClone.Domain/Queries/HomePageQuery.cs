using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class HomePageQuery : IRequest<HomePageQueryResult>
{
    public string Username { get; init; }
}

public class HomePageQueryResult
{
    public ICollection<Post> HomepagePosts { get; set; }
}

internal class HomePageQueryHandler : IRequestHandler<HomePageQuery, HomePageQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public HomePageQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<HomePageQueryResult> Handle(HomePageQuery request, CancellationToken cancellationToken)
    {
        if (!(await _dbContext.Users.AnyAsync(u => u.Username == request.Username)))
        {
            //probably should be exception
            return null;
        }

        var userFollowings = await _dbContext.Followings
            .Where(f => f.FollowByUsername == request.Username)
            .Include(f => f.FollowForUser.Posts)
            .ToListAsync();

        var userOwnPosts = await _dbContext.Posts
            .Where(p => p.AuthorUsername == request.Username)
            .ToListAsync();
        
        var homepagePosts = new List<Post>();
        homepagePosts.AddRange(userOwnPosts);
        homepagePosts.AddRange(userFollowings.SelectMany(f => f.FollowForUser.Posts).ToList());
        
        return new HomePageQueryResult
        {
            HomepagePosts = homepagePosts.OrderByDescending(p => p.PostId).ToList()
        };
    }
}