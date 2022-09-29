using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

[Route("/posts")]
public class MakeCommentController : BaseController
{
    [HttpGet("{postId}/make-comment")]
    public ContentResult GetLikePage([FromRoute] int postId, CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/make-comment.html");
        html = html.Replace("{{postId}}", postId.ToString());

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}