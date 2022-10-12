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

[Route("api/posts")]
public class PostController : BaseController
{
    private readonly IMediator _mediator;

    public PostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public Task<IActionResult> CreatePost([FromBody] CreatePostRequest request, CancellationToken cancellationToken) =>
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

            var command = new CreatePostCommand
            {
                AuthorUsername = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                CommentTo = request.CommentTo,
                Message = request.Message
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.PostIsCreated)
            {
                if (!result.AuthorExists)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.UserNotFound,
                        Message = "author doest not exist"
                    });
                }

                if (!result.OriginPostExists)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.OriginPostNotFound,
                        Message = "origin post not exists"
                    });
                }

                if (result.MessageIsEmpty)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.InvalidMessage,
                        Message = "messega cound not be empty"
                    });
                }
            }

            var response = new CreatePostResponse
            {
                PostIsCreated = result.PostIsCreated,
                Post = result.Post
            };

            return Created("http://todo.com", response);
        }, cancellationToken);

    [HttpGet("{postId}")]
    public Task<IActionResult> GetPost([FromRoute] int postId, CancellationToken cancellationToken) =>
        SafeExecute(async () =>
        {
            var query = new PostQuery
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

            var response = new GetPostResponse
            {
                Post = result.Post,
                LikedByUsername = result.LikedByUsername,
                Comments = result.Comments
            };

            return Ok(response);
        }, cancellationToken);

    [HttpDelete]
    public Task<IActionResult> DeletePost([FromBody] DeletePostRequest request, CancellationToken cancellationToken) =>
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

            var command = new DeletePostCommand
            {
                Username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                PostId = request.PostId
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.DeleteIsSuccessful)
            {
                if (!result.PostExist)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.PostNotFound,
                        Message = "post not found"
                    });
                }

                if (result.AccessIsDenied)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.ExecutAccessForbidden,
                        Message = "only author can delete post"
                    });
                }
            }

            var response = new DeletePostResponse
            {
                DeleteIsSuccessful = result.DeleteIsSuccessful
            };

            return Ok(response);
        }, cancellationToken);
}