using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TwitterClone.Contracts.Http;
using TwitterClone.Domain.Commands;

namespace TwitterClone.Api.Controllers;

[Route("api/likes")]
public class LikeController : BaseController
{
    private readonly IMediator _mediator;

    public LikeController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch]
    public Task<IActionResult> LikePost([FromBody] LikePostRequest request, CancellationToken cancellationToken) =>
        SafeExecute(async () =>
        {
            var command = new LikePostCommand
            {
                LikedPostId = request.LikedPostId,
                LikedByUsername = request.LikedByUsername
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new LikePostResponse
            {
                LikeStatusIsChanged = result.LikeStatusIsChanged
            };

            return Created("http://todo.com", response);
        }, cancellationToken);
}