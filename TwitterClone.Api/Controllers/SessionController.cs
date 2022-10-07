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
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace TwitterClone.Api.Controllers
{
    
    public class SessionController : BaseController
    {
        private readonly IMediator _mediator;

        public SessionController (IMediator mediator)
        {
            _mediator = mediator;
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
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.UserNotFound,
                        Message = "user not found"
                    });
                }

                if(!response.PasswordIsCorrect)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Code = ErrorCode.WrongPassword,
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

        [HttpGet("sign-out")]
        public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }        
    }    
}