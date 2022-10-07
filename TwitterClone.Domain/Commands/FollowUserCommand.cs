using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TwitterClone.Domain.Commands;

public class FollowUserCommand : IRequest<FollowUserCommandResult>
{
    public string FollowByUsername { get; set; }
    public string FollowForUsername { get; set; }
}

public class FollowUserCommandResult
{
    public bool FollowStatusIsChanged { get; init; }
    public bool FollowByUserExists { get; init; }
    public bool FollowForUserExists { get; init; }
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

        var followByUserExists = await _dbContext.Users.AnyAsync(u => u.Username == request.FollowByUsername);
        var followForUserExists = await _dbContext.Users.AnyAsync(u => u.Username == request.FollowForUsername);

        if (!followByUserExists || !followForUserExists)
        {
            return new FollowUserCommandResult
            {
                FollowStatusIsChanged = false,
                FollowByUserExists = followByUserExists,
                FollowForUserExists = followForUserExists
            };
        }

        if (await _dbContext.Followings.Where(f => f.FollowByUsername == request.FollowByUsername && f.FollowForUsername == request.FollowForUsername).CountAsync() == 0)
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