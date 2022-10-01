using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

public class FavoritesPageController : BaseController
{
    [HttpGet("/favorites")]
    public ContentResult GetFavoritesPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/favorites.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}