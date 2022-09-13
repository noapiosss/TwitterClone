using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using MediatR;
using Shouldly;

using Microsoft.EntityFrameworkCore;

using TwitterClone.Domain.Commands;
using TwitterClone.Domain.Database;
using System.IO;

namespace TwitterClone.UnitTests.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly TwitterCloneDbContext _dbContext;
    private readonly IRequestHandler<CreateUserCommand, CreateUserCommandResult> _handler;

    public CreateUserCommandHandlerTests()
    {
        var tempFile = Path.GetTempFileName();
        var options = new DbContextOptionsBuilder<TwitterCloneDbContext>()
            .UseSqlite($"Data Source={tempFile};")
            .Options;

        _dbContext = new TwitterCloneDbContext(options);
        _dbContext.Database.Migrate();

        _handler = new CreateUserCommandHandler(_dbContext);
    }

    [Fact]
    public async Task HandleShouldCreateUser()
    {
        // Arrange
        var newUsername = Guid.NewGuid().ToString();
        var newEmail = Guid.NewGuid().ToString();
        var newPassword = Guid.NewGuid().ToString();
        var command = new CreateUserCommand
        {
            Username = newUsername,
            Email = newEmail,
            Password = newPassword
        };        

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsRegistrationSuccessful.ShouldBeTrue();
    }

    [Fact]
    public async Task HandleUsersAlreadyExists()
    {
        // Arrange
        var firsetUsername = Guid.NewGuid().ToString();
        var firstEmail = Guid.NewGuid().ToString();
        var firstPassword = Guid.NewGuid().ToString();

        var secondUsername = firsetUsername;
        var secondEmail = Guid.NewGuid().ToString();
        var secondPassword = Guid.NewGuid().ToString();

        var thirdUsername = Guid.NewGuid().ToString();
        var thirdEmail = firstEmail;
        var thirdPassword = Guid.NewGuid().ToString();
        
        var addFirstUserCommand = new CreateUserCommand
        {
            Username = firsetUsername,
            Email = firstEmail,
            Password = firstPassword
        };
        
        var addScondUserCommand = new CreateUserCommand
        {
            Username = secondUsername,
            Email = secondEmail,
            Password = secondPassword
        };

        var addThirdUserCommand = new CreateUserCommand
        {
            Username = thirdUsername,
            Email = thirdEmail,
            Password = thirdPassword
        };

        // Act
        var addFirstUserResult = await _handler.Handle(addFirstUserCommand, CancellationToken.None);
        var addSecondUserResult = await _handler.Handle(addScondUserCommand, CancellationToken.None);
        var addThirdUserResult = await _handler.Handle(addThirdUserCommand, CancellationToken.None);

        // Assert
        addSecondUserResult.IsRegistrationSuccessful.ShouldBeFalse();
        addThirdUserResult.IsRegistrationSuccessful.ShouldBeFalse();
    }
}