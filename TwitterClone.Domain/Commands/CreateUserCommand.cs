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
    public string ReturnMessage { get; set; }
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
            Password = request.Password.GetHashCode().ToString()
        };

        
        var usersList = _dbContext.Users.ToListAsync().Result.ToList();

        if (usersList.Any(u => u.Username == user.Username || u.Email == user.Email))
        {
            return new CreateUserCommandResult
            {
                ReturnMessage = "User already exist"
            };
        }

        await _dbContext.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return new CreateUserCommandResult
        {
            ReturnMessage = "Successful registration"
        };
    }
}