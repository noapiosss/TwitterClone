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
    public bool PostIsCreated { get; init; }
}

internal class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, CreatePostCommandResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public CreatePostCommandHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<CreatePostCommandResult> Handle(CreatePostCommand request, CancellationToken cancellationToken = default)
    {
        var userIsExists = _dbContext.Users.Any(u => u.Username == request.AuthorUsername) ? true : false;
        var originPostIsExists = true;
        if (request.CommentTo != null)
        {
            originPostIsExists = _dbContext.Posts.Any(p => p.PostId == request.CommentTo) ? true : false;
        }
        var messageIsEmpty = String.IsNullOrWhiteSpace(request.Message);

        if (!userIsExists || !originPostIsExists || messageIsEmpty)
        {
            return new CreatePostCommandResult
            {
                PostIsCreated = false
            };
        }

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
            PostIsCreated = true
        };
    }
}