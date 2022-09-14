using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TwitterClone.Contracts.Http;
using TwitterClone.Domain.Commands;

namespace TwitterClone.Api.Controllers;

[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password.GetHashCode().ToString()
        };

        var result = await _mediator.Send(command, cancellationToken);
        var response = new CreateUserResponse
        {
            IsRegistrationSuccessful = result.IsRegistrationSuccessful
        };
        
        return Created("http://todo.com", response);
    }
}