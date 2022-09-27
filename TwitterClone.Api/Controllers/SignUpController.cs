using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

[Route("/sign-up")]
public class SignUpController : BaseController
{
    [HttpGet]
    public ContentResult GeSignUpPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/sign-up.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}