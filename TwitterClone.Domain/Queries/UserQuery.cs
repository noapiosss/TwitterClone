<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
=======
>>>>>>> 8795c817b61f1fec30c7164f3cc39b3f582c398c
using System.Threading;
using System.Threading.Tasks;

using MediatR;

<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;

=======
>>>>>>> 8795c817b61f1fec30c7164f3cc39b3f582c398c
using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;

namespace TwitterClone.Domain.Queries;

public class UserQuery : IRequest<UserQueryResult>
{
    public string Username { get; init; }
}

public class UserQueryResult
{
<<<<<<< HEAD
    public List<Post> UserPosts { get; set; }
=======
    public User User { get; set; }
>>>>>>> 8795c817b61f1fec30c7164f3cc39b3f582c398c
}

public class UserQueryHandler : IRequestHandler<UserQuery, UserQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public UserQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }

<<<<<<< HEAD
    public async Task<UserQueryResult> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        var userPosts = _dbContext.Posts.ToListAsync().Result.ToList().Where(p => p.AuthorUsername == request.Username).ToList();

        return new UserQueryResult
        {
            UserPosts = userPosts
        };
=======
    public Task<UserQueryResult> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
>>>>>>> 8795c817b61f1fec30c7164f3cc39b3f582c398c
    }
}