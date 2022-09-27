using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

[Route("/sign-in")]
public class SignInController : BaseController
{
    [HttpGet]
    public ContentResult GeSignInPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/sign-in.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}