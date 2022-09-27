using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

[Route("/registration")]
public class RegistrationController : BaseController
{
    [HttpGet]
    public ContentResult GetUserPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/registration.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}