using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
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
                Password = request.Password
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new CreateUserResponse
            {
                IsRegistrationSuccessful = result.IsRegistrationSuccessful,
                UsernameIsAlreadyInUse = result.UsernameIsAlreadyInUse,
                EmailIsAlreadyInUse = result.EmailIsAlreadyInUse
            };

            return Created("http://todo.com", response);
        }, cancellationToken);

    [HttpGet("{username}/posts")]
    public Task<IActionResult> GetUserPosts([FromRoute] string username, CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new UserPostsQuery
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

    [HttpGet("homepage")]
    [Authorize]
        public Task<IActionResult> GetHomePagePosts(CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new HomePageQuery
            {
                Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetHomePagePostsResponse
            {                
                HomepagePosts = result.HomepagePosts
            };

            return Ok(response);
        }, cancellationToken);

    [HttpGet("favorites")]
    [Authorize]
        public Task<IActionResult> GetFavoritesPostsPosts(CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new FavoritesQuery
            {
                Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetFavoritesPostsResponse
            {                
                FavoritesPosts = result.FavoritesPosts
            };

            return Ok(response);
        }, cancellationToken);
    
    [HttpGet("username")]
        public Task<IActionResult> GetSessionUsername(CancellationToken cancellationToken) =>
        SafeExecute( async () => 
        {
            var username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;            
            var response = new {username};

            return Ok(response);
        }, cancellationToken);     
}