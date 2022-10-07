using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class PostQuery : IRequest<PostQueryResult>
{
    public int PostId { get; init; }
}

public class PostQueryResult
{
    public Post Post { get; set; }
    public ICollection<string> LikedByUsername { get; set; }
    public ICollection<Post> Comments { get; set; }
}

internal class PostQueryHandler : IRequestHandler<PostQuery, PostQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public PostQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PostQueryResult> Handle(PostQuery request, CancellationToken cancellationToken)
    {
        var post = await _dbContext.Posts
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.PostId == request.PostId);

        if (post == null)
        {
            return null;
        }

        var comments = await _dbContext.Posts
            .Where(p => p.CommentTo == request.PostId)
            .OrderByDescending(p => p.PostId)
            .ToListAsync();

        return new PostQueryResult
        {
            Post = new Post
            {
                PostId = post.PostId,
                AuthorUsername = post.AuthorUsername,
                PostDate = post.PostDate,
                Message = post.Message
            },
            LikedByUsername = post.Likes.Select(l => l.LikedByUsername).ToList(),
            Comments = comments
        };
    }
}