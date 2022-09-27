using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

[Route("/users")]
public class UserPageController : BaseController
{
    [HttpGet("{username}")]
    public ContentResult GetUserPage([FromRoute] string username, CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/users.html");
        html = html.Replace("{{username}}", username);

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}