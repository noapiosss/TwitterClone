using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

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
    public Post Post { get; init; }
    public bool AuthorExists { get; init; }
    public bool OriginPostExists { get; init; }
    public bool MessageIsEmpty { get; init; }
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
        var authorExists = await _dbContext.Users.AnyAsync(u => u.Username == request.AuthorUsername, cancellationToken);
        var originPostExists = true;
        if (request.CommentTo != null)
        {
            originPostExists = await _dbContext.Posts.AnyAsync(p => p.PostId == request.CommentTo, cancellationToken);
        }
        var messageIsEmpty = string.IsNullOrWhiteSpace(request.Message);

        if (!authorExists || !originPostExists || messageIsEmpty)
        {
            return new CreatePostCommandResult
            {
                PostIsCreated = false,
                AuthorExists = authorExists,
                OriginPostExists = originPostExists,
                MessageIsEmpty = messageIsEmpty
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
            PostIsCreated = true,
            Post = post
        };
    }
}