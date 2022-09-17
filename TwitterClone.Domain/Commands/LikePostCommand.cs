using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using System.Collections.Generic;
using System.Linq;

namespace TwitterClone.Domain.Commands;

public class LikePostCommand : IRequest<LikePostCommandResult>
{
    public int LikedPostId { get; set; }
    public string LikedByUsername { get; set; }
}

public class LikePostCommandResult
{
    public bool LikeStatusIsChanged { get; init; }
}

public class LikePostCommandHandler : IRequestHandler<LikePostCommand, LikePostCommandResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public LikePostCommandHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<LikePostCommandResult> Handle(LikePostCommand request, CancellationToken cancellationToken = default)
    {
        var like = new Like
        {
            LikedPostId = request.LikedPostId,
            LikedByUsername = request.LikedByUsername
        };

        if (_dbContext.Likes.Where(l => l.LikedPostId == request.LikedPostId && l.LikedByUsername == request.LikedByUsername).Count() == 0)
        {
            await _dbContext.AddAsync(like, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        } 
        else
        {
            _dbContext.Likes.Attach(like);
            _dbContext.Likes.Remove(like);
            _dbContext.SaveChanges();
        }

        return new LikePostCommandResult
        {
            LikeStatusIsChanged = true
        };
    }
}