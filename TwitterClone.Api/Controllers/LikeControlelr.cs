using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TwitterClone.Contracts.Http;
using TwitterClone.Domain.Commands;
using TwitterClone.Domain.Queries;

namespace TwitterClone.Api.Controllers;

[Route("api/likes")]
public class LikeController : BaseController
{
    private readonly IMediator _mediator;

    public LikeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPatch]
    public Task<IActionResult> LikePost([FromBody] LikePostRequest request, CancellationToken cancellationToken) =>
        SafeExecute(async () =>
        {
            if (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name) == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.Unauthorized,
                    Message = "current request require authorization"
                });
            }

            var command = new LikePostCommand
            {
                LikedPostId = request.LikedPostId,
                LikedByUsername = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.LikeStatusIsChanged)
            {
                if (!result.PostExists)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.PostNotFound,
                        Message = "post not found"
                    });
                }

                if (!result.UserExists)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.UserNotFound,
                        Message = "user not exists"
                    });
                }
            }

            var response = new LikePostResponse
            {
                LikeStatusIsChanged = result.LikeStatusIsChanged
            };

            return Ok(response);
        }, cancellationToken);

    [HttpGet("{postId}")]
    public Task<IActionResult> GetPostLikes([FromRoute] int postId, CancellationToken cancellationToken) =>
        SafeExecute(async () =>
        {
            var query = new LikeQuery
            {
                PostId = postId
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.PostNotFound,
                    Message = "post not found"
                });
            }

            var response = new GetLikesResponse
            {
                UsersThatLikePost = result.UsersThatLikePost
            };

            return Ok(response);
        }, cancellationToken);

    [HttpGet]
    public Task<IActionResult> GetUserLikes(CancellationToken cancellationToken) =>
        SafeExecute(async () =>
        {
            if (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name) == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.Unauthorized,
                    Message = "current request require authorization"
                });
            }

            var query = new UserLikesQuery
            {
                Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.PostNotFound,
                    Message = "post not found"
                });
            }

            var response = new GetPostsThatUserLikeResponse
            {
                PostIdsThatUserLike = result.PostIdsThatUserLike
            };

            return Ok(response);
        }, cancellationToken);
}