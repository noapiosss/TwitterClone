using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.Api.Controllers;

public class WebPagesController : BaseController
{
    [HttpGet("sign-up")]
    public ContentResult GetSignUpPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/sign-up.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }        
    
    [HttpGet("sign-in")]
    public ContentResult GetSignInPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/sign-in.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }

    
    [HttpGet("home")]
    public ContentResult GetHomePage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/homepage.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }

    [HttpGet("favorites")]
    public ContentResult GetFavoritesPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/favorites.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }

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

    [HttpGet("posts/{postId}")]
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

    [HttpGet("posts/{postId}/make-comment")]
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

    [HttpGet("posts/{postId}/likes")]
    public ContentResult GetLikePage([FromRoute] int postId, CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/likes.html");
        html = html.Replace("{{postId}}", postId.ToString());

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }

    [HttpGet("users/{username}")]
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

    [HttpGet("users/{username}/followings")]
    public ContentResult GetUserFollowingsPage([FromRoute] string username, CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/followings.html");
        html = html.Replace("{{username}}", username);

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }

    [HttpGet("users/{username}/followers")]
    public ContentResult GetUserFollowersPage([FromRoute] string username, CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/followers.html");
        html = html.Replace("{{username}}", username);

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }
}