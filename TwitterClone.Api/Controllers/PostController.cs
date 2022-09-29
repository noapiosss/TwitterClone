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

    public PostController (IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public Task<IActionResult> CreatePost([FromBody] CreatePostRequest request, CancellationToken cancellationToken) =>
        SafeExecute(async () =>
        {
            var command = new CreatePostCommand
            {
                AuthorUsername = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                CommentTo = request.CommentTo,
                Message = request.Message
            };

            var result = await _mediator.Send(command, cancellationToken);
            var response = new CreatePostResponse
            {
                Post = result.Post
            };

            return Created("http://todo.com", response);
        }, cancellationToken);

    [HttpGet("{postId}")]
    public Task<IActionResult> GetUserPosts([FromRoute] int postId, CancellationToken cancellationToken) =>
        SafeExecute(async () => 
        {
            var query = new PostQuery
            {
                PostId = postId
            };

            var result = await _mediator.Send(query, cancellationToken);
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
            var command = new DeletePostCommand
            {
                PostId = request.PostId
            };

            // var a = HttpContext.User.Claims;

            var result = await _mediator.Send(command, cancellationToken);
            var response = new DeletePostResponse
            {
                IsDeleteSuccessful = result.IsDeleteSuccessful
            };

            return Ok(response);
        }, cancellationToken);
}