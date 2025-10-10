using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;
using Uniring.Contracts.Auth;

namespace Uniring.Api.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiControllerBase
    {
        private readonly IIdentityService _identity;

        public AccountController(IIdentityService identity) => _identity = identity;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var (succeeded, errors) = await _identity.RegisterUserAsync(req);
            if (!succeeded) return BadRequest(new { errors });
            RegisterResponse resp = new RegisterResponse("Token", null, null);
            return Ok(resp);
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
