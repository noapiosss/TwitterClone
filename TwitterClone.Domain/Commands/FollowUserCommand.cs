using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using System.Collections.Generic;
using System.Linq;

namespace TwitterClone.Domain.Commands;

public class FollowUserCommand : IRequest<FollowUserCommandResult>
{
    public string FollowByUsername { get; set; }
    public string FollowForUsername { get; set; }
}

public class FollowUserCommandResult
{
    public bool FollowStatusIsChanged { get; init; }
}

internal class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, FollowUserCommandResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public FollowUserCommandHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<FollowUserCommandResult> Handle(FollowUserCommand request, CancellationToken cancellationToken = default)
    {
        var following = new Following
        {
            FollowByUsername = request.FollowByUsername,
            FollowForUsername = request.FollowForUsername
        };

        if (_dbContext.Followings.Where(f => f.FollowByUsername == request.FollowByUsername && f.FollowForUsername == request.FollowForUsername).Count() == 0)
        {
            await _dbContext.AddAsync(following, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        } 
        else
        {
            _dbContext.Followings.Attach(following);
            _dbContext.Followings.Remove(following);
            _dbContext.SaveChanges();
        }

        return new FollowUserCommandResult
        {
            FollowStatusIsChanged = true
        };
    }
}