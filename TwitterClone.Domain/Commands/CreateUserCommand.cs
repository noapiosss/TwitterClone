using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TwitterClone.Contracts.Database;
using TwitterClone.Domain.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TwitterClone.Domain.Helpers;

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
    public bool UsernameIsAlreadyInUse { get; set; }
    public bool EmailIsAlreadyInUse { get; set; }

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
        var usernameIsAlreadyInUse = _dbContext.Users.Any(u => u.Username == request.Username) ? true : false;
        var emailIsAlreadyInUse = _dbContext.Users.Any(u => u.Email == request.Email) ? true : false;

        if (usernameIsAlreadyInUse || emailIsAlreadyInUse)
        {
            return new CreateUserCommandResult
            {
                IsRegistrationSuccessful = false,
                UsernameIsAlreadyInUse = usernameIsAlreadyInUse,
                EmailIsAlreadyInUse = emailIsAlreadyInUse,
            };
        }

        var user = new User 
        {
            Username = request.Username,
            Email = request.Email,
            Password = PasswordHelper.HashString(request.Password)
        };

        await _dbContext.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return new CreateUserCommandResult
        {
            IsRegistrationSuccessful = true,
            UsernameIsAlreadyInUse = usernameIsAlreadyInUse,
            EmailIsAlreadyInUse = emailIsAlreadyInUse,
        };
    }
}