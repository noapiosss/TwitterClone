using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TwitterClone.Contracts.Http;
using TwitterClone.Domain.Commands;
using TwitterClone.Domain.Queries;

namespace TwitterClone.Api.Controllers;

[Route("api/followings")]
public class FollowingController : BaseController
{
    private readonly IMediator _mediator;

    public FollowingController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch]
    public Task<IActionResult> FollowingPost([FromBody] FollowUserCommand request, CancellationToken cancellationToken) =>
        SafeExecute(async () =>
        {
            var command = new FollowUserCommand
            {
                FollowByUsername = request.FollowByUsername,
                FollowForUsername = request.FollowForUsername
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new FollowUserResponse
            {
                FollowStatusIsChanged = result.FollowStatusIsChanged
            };

            return Created("http://todo.com", response);
        }, cancellationToken);
}