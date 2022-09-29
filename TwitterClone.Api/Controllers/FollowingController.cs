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

[Route("api")]
public class FollowingController : BaseController
{
    private readonly IMediator _mediator;

    public FollowingController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch("follow")]
    [Authorize]
    public Task<IActionResult> FollowingPost([FromBody] FollowUserCommand request, CancellationToken cancellationToken) =>
        SafeExecute(async () =>
        {
            var command = new FollowUserCommand
            {
                FollowByUsername = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                FollowForUsername = request.FollowForUsername
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new FollowUserResponse
            {
                FollowStatusIsChanged = result.FollowStatusIsChanged
            };

            return Ok(response);
        }, cancellationToken);

    
    [HttpGet("followings")]
    [Authorize]
    public Task<IActionResult> GetFollowings(CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new FollowingsQuery
            {
                Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetFollowingsResponse
            {                
                Followings = result.Followings
            };

            return Ok(response);
        }, cancellationToken);

    [HttpGet("followers")]
    [Authorize]
    public Task<IActionResult> GetFollowers(CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new FollowersQuery
            {
                Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = new GetFollowersResponse
            {                
                Followers = result.Followers
            };

            return Ok(response);
        }, cancellationToken);

}