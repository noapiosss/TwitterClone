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

public class HomePageQueryHandler : IRequestHandler<HomePageQuery, HomePageQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public HomePageQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<HomePageQueryResult> Handle(HomePageQuery request, CancellationToken cancellationToken)
    {
        var usersFromFollowings = _dbContext.Followings
            .Where(f => f.FollowByUsername == request.Username)
            .Include(f => f.FollowForUser.Posts)
            .ToList();

        var userOwnPosts = _dbContext.Posts
            .Where(p => p.AuthorUsername == request.Username)
            .ToList();
        
        var homepagePosts = new List<Post>();
        homepagePosts.AddRange(userOwnPosts);

        foreach (var userFromFollowing in usersFromFollowings)
        {
            foreach (var post in userFromFollowing.FollowForUser.Posts)
            {
                homepagePosts.Add(new Post
                {
                    PostId = post.PostId,
                    AuthorUsername = post.AuthorUsername,
                    PostDate = post.PostDate,
                    Message = post.Message
                });
            }
        }
        
        return new HomePageQueryResult
        {
            HomepagePosts = homepagePosts.OrderByDescending(p => p.PostId).ToList()
        };
    }
}