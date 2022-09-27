using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

[Route("/posts")]
public class PostPageController : BaseController
{
    [HttpGet("{postId}")]
    public ContentResult GetPostPage([FromRoute] int postId, CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/posts.html");
        html = html.Replace("{{postId}}", postId.ToString());

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}