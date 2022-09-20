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

    [HttpGet("{username}/posts")]
    public Task<IActionResult> GetUserPosts([FromRoute] string username, CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new UserQuery
            {
                Username = username
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetUserPostsResponse
            {                
                UserPosts = result.UserPosts
            };

            return Ok(response);
        }, cancellationToken);
    
    [HttpGet("{username}/followings")]
    public Task<IActionResult> GetFollowings([FromRoute] string username, CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new FollowingsQuery
            {
                Username = username
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetFollowingsResponse
            {                
                Followings = result.Followings
            };

            return Ok(response);
        }, cancellationToken);

    [HttpGet("{username}/followers")]
    public Task<IActionResult> GetFollowers([FromRoute] string username, CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new FollowersQuery
            {
                Username = username
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetFollowersResponse
            {                
                Followers = result.Followers
            };

            return Ok(response);
        }, cancellationToken);

    [HttpGet("{username}/homePagePosts")]
    public Task<IActionResult> GetHomePagePosts([FromRoute] string username, CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new HomePageQuery
            {
                Username = username
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetHomePagePostsResponse
            {                
                PostsFromFollowings = result.PostsFromFollowings
            };

            return Ok(response);
        }, cancellationToken);
}