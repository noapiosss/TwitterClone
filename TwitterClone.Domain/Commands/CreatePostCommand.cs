using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> 8795c817b61f1fec30c7164f3cc39b3f582c398c

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
<<<<<<< HEAD
        var commentTo = (request.CommentTo == 0) ? null : request.CommentTo;
        var post = new Post
        {
            AuthorUsername = request.AuthorUsername,
            CommentTo = commentTo,
            PostDate = DateTime.UtcNow,
            Message = request.Message,
            Likes = new List<Like>()
=======

        var post = new Post
        {
            AuthorUsername = request.AuthorUsername,
            CommentTo = request.CommentTo,
            PostDate = DateTime.UtcNow,
            Message = request.Message
>>>>>>> 8795c817b61f1fec30c7164f3cc39b3f582c398c
        };

        await _dbContext.AddAsync(post, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return new CreatePostCommandResult
        {
            Post = post
        };
    }
}