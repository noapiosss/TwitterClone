using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

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
        try
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
        catch (InvalidOperationException ioe) when (ioe.InnerException is NpgsqlException)
        {
            var response = new ErrorResponse
            {
                Code = ErrorCode.DbFailureError,
                Message = "DB failure"
            };

            return ToActionResult(response);
        }
        catch (Exception)
        {
            var response = new ErrorResponse
            {
                Code = ErrorCode.InternalServerError,
                Message = "Unhandled error"
            };

            return ToActionResult(response);
        }
    }

    private IActionResult ToActionResult(ErrorResponse errorResponse)
    {
        return StatusCode((int)errorResponse.Code / 100, errorResponse);
    }
}