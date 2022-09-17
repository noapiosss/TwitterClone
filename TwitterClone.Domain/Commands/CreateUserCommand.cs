using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TwitterClone.Domain.Commands;

public class CreateUserCommand : IRequest<CreateUserCommandResult>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class CreateUserCommandResult
{
    public bool IsRegistrationSuccessful { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResult>
{
    private readonly TwitterCloneDbContext _dbContext;

    public CreateUserCommandHandler(TwitterCloneDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<CreateUserCommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = new User 
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password.GetHashCode().ToString() // :( fix it
        };

        
        var usersList = await _dbContext.Users.ToListAsync(cancellationToken); 

        if (usersList.Any(u => u.Username == user.Username || u.Email == user.Email)) // refact as db req
        {
            return new CreateUserCommandResult
            {
                IsRegistrationSuccessful = false
            };
        }

        await _dbContext.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return new CreateUserCommandResult
        {
            IsRegistrationSuccessful = true
        };
    }
}