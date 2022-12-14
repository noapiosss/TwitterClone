using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Domain.Database;
using TwitterClone.Domain.Helpers;

namespace TwitterClone.Domain.Queries;

public class SignInQuery : IRequest<SignInQueryResult>
{
    public string Username { get; init; }
    public string Password { get; init; }
}

public class SignInQueryResult
{
    public bool IsAuthenticated { get; set; }
    public bool UserIsFound { get; set; }
    public bool PasswordIsCorrect { get; set; }
}

internal class SignInQueryHandler : IRequestHandler<SignInQuery, SignInQueryResult>
{
    private readonly TwitterCloneDbContext _dbContext;


    public SignInQueryHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<SignInQueryResult> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null)
        {
            return new SignInQueryResult
            {
                IsAuthenticated = false,
                UserIsFound = false,
                PasswordIsCorrect = false
            };
        }

        if (user.Password != PasswordHelper.GetPasswordHash(request.Password))
        {
            return new SignInQueryResult
            {
                IsAuthenticated = false,
                UserIsFound = true,
                PasswordIsCorrect = false
            };
        }

        return new SignInQueryResult
        {
            IsAuthenticated = true,
            UserIsFound = true,
            PasswordIsCorrect = true
        };
    }
}