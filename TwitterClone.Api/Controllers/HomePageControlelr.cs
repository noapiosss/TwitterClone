using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

public class HomePageController : BaseController
{
    [HttpGet("/home")]
    public ContentResult GetHomePage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/homepage.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}