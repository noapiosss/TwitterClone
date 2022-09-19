using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using System.Collections.Generic;
using System.Linq;

namespace TwitterClone.Domain.Commands;

public class CreatePostCommand : IRequest<CreatePostCommandResult>
{
    public string AuthorUsername { get; set; }
    public int? CommentTo { get; init; }
    public string Message { get; init; }
}

public class CreatePostCommandResult
{
    public Post Post { get; init; }
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CreatePostCommandResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public CreatePostCommandHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<CreatePostCommandResult> Handle(CreatePostCommand request, CancellationToken cancellationToken = default)
    {
        var post = new Post
        {
            AuthorUsername = request.AuthorUsername,
            CommentTo = request.CommentTo,
            PostDate = DateTime.UtcNow,
            Message = request.Message,
        };

        await _dbContext.AddAsync(post, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return new CreatePostCommandResult
        {
            Post = post
        };
    }
}