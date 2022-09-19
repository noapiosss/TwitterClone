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
}

public class PostQueryHandler : IRequestHandler<PostQuery, PostQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public PostQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<PostQueryResult> Handle(PostQuery request, CancellationToken cancellationToken)
    {
        var post = _dbContext.Posts
            .Include(p => p.Likes)
            .First(p => p.PostId == request.PostId);

        return new PostQueryResult
        {
            Post = post
        };
    }
}