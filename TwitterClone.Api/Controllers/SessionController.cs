using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using TwitterClone.Contracts.Http;
using TwitterClone.Domain.Queries;

namespace TwitterClone.Api.Controllers
{
    
    public class SessionController : BaseController
    {
        private readonly IMediator _mediator;

        public SessionController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/[controller]")]        
        public IEnumerable<string> GetSessionInfo()
        {
            List<string> sessionInfo = new List<string>();

            if(string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionVariables.SessionKeyUsername)));
            {
                HttpContext.Session.SetString(SessionsKeyEnum.SessionKeyUsername.ToString(), "Current User");
                HttpContext.Session.SetString(SessionsKeyEnum.SessionKeySessionId.ToString(), Guid.NewGuid().ToString());
            }

            var username = HttpContext.Session.GetString(SessionVariables.SessionKeyUsername);
            var sessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

            sessionInfo.Add(username);
            sessionInfo.Add(sessionId);


            return sessionInfo;
        }

    [HttpGet("sign-in")]
    public ContentResult GeSignInPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/sign-in.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var query = new SignInQuery
        {
            Username = request.Username,
            Password = request.Password
        };

        var result = await _mediator.Send(query, cancellationToken);
        var response = new SignInResponse
        {
            IsAuthenticated = result.IsAuthenticated,
            UserIsFound = result.UserIsFound,
            PasswordIsCorrect = result.PasswordIsCorrect
        };

        if (!response.IsAuthenticated)
        {
            if(!response.UserIsFound)
            {
                return BadRequest(new 
                {
                    Message = "user not found"
                });
            }

            if(!response.PasswordIsCorrect)
            {
                return BadRequest(new
                {
                    Message = "wrong password"
                });
            }
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authProperties);

        return Ok();
    
    }

    [HttpPost("sign-out")]
    public Task<IActionResult> SignOut(CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpGet("sign-up")]
    public ContentResult GeSignUpPage(CancellationToken cancellationToken)
    {
        var html = System.IO.File.ReadAllText("wwwroot/sign-up.html");

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }

    [HttpPost("sign-up")]
    public Task<IActionResult> SignUp(CancellationToken cancellationToken)
    {
        return Task.FromResult<IActionResult>(Ok());
    }
    }

    
}