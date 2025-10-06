using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;
using Uniring.Contracts.Auth;
using Uniring.Domain.Entities;

namespace Uniring.Api.Controllers
{

    public class AccountController : ApiControllerBase
    {
        private readonly IIdentityService _identity;

        public AccountController(IIdentityService identity) => _identity = identity;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var (succeeded, errors) = await _identity.RegisterAsync(req, role: "User");
            if (!succeeded) return BadRequest(new { errors });
            return Ok();
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest req)
        //{
        //    var res = await _identity.LoginAsync(req);
        //    if (!res.Success) return Unauthorized(new { res.Errors });
        //    return Ok(new { token = res.Token, expires = res.ExpiresAt });
        //}
    }

}
