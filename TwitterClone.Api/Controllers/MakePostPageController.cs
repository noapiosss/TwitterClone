using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

public class MakePostPageController : BaseController
{
    [HttpGet("make-post")]
    public ContentResult GetMakePostPage([FromRoute] int postId, CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/make-post.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}