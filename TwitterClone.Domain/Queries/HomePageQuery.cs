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
    public ICollection<Post> PostsFromFollowings { get; set; }
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

        var postsFromFollowings = new List<Post>();

        foreach (var userFromFollowing in usersFromFollowings)
        {
            foreach (var post in userFromFollowing.FollowForUser.Posts)
            {
                postsFromFollowings.Add(new Post
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
            PostsFromFollowings = postsFromFollowings
        };
    }
}