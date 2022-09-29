using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

[Route("/posts")]
public class MakeCommentPageController : BaseController
{
    [HttpGet("{postId}/make-comment")]
    public ContentResult GetMakeCommentPage([FromRoute] int postId, CancellationToken cancellationToken)
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