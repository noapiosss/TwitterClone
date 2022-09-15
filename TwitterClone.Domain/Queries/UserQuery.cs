using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class UserQuery : IRequest<UserQueryResult>
{
    public string Username { get; init; }
}

public class UserQueryResult
{
    public User User { get; set; }
}

public class UserQueryHandler : IRequestHandler<UserQuery, UserQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public UserQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserQueryResult> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}