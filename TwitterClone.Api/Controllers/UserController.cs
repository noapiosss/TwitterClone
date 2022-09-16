using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TwitterClone.Contracts.Http;
using TwitterClone.Domain.Commands;
using TwitterClone.Domain.Queries;

namespace TwitterClone.Api.Controllers;

[Route("api/users")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;

    public UserController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken) =>
        SafeExecute(async () =>
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
        }, cancellationToken);

    [HttpGet("{username}")]

    public Task<IActionResult> GetUserPosts([FromRoute] string username, CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new UserQuery
            {
                Username = username
            };

            var result = await _mediator.Send(query, cancellationToken);
            var userPosts = result.UserPosts;
            var response = new GetUserPostsResponse
            {
                UserPosts = userPosts
            };

            return Ok(response);
        }, cancellationToken);
    
}