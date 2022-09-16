using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TwitterClone.Domain.Commands;

public class DeletePostCommand : IRequest<DeletePostCommandResult>
{
    public string Username { get; set; }
    public int PostId { get; set; }
}

public class DeletePostCommandResult
{
    public bool IsDeleteSuccessful { get; init; }
}

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, DeletePostCommandResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public DeletePostCommandHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<DeletePostCommandResult> Handle(DeletePostCommand request, CancellationToken cancellationToken = default)
    {
        var post = _dbContext.Posts.ToListAsync().Result.ToList().First(p => p.PostId == request.PostId);

        if (post.AuthorUsername != request.Username)
        {
            return new DeletePostCommandResult
            {
                IsDeleteSuccessful = false
            };
        };

        _dbContext.Posts.Attach(post);
        _dbContext.Posts.Remove(post);
        _dbContext.SaveChanges();

        return new DeletePostCommandResult
        {
            IsDeleteSuccessful = true
        };
    }
}