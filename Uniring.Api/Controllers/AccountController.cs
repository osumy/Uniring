using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uniring.Api.Authentication;
using Uniring.Application.Interfaces;
using Uniring.Contracts.ApiResult;
using Uniring.Contracts.Auth;

namespace Uniring.Api.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiControllerBase
    {
        private readonly IIdentityService _identity;
        private readonly IJwtGenerator _jwtGenerator;

        public AccountController(IIdentityService identity, IJwtGenerator jwtGenerator)
        {
            _identity = identity;
            _jwtGenerator = jwtGenerator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest req)
        {
            var (succeeded, errors) = await _identity.RegisterUserAsync(req);
            if (!succeeded) return BadRequest(new { errors });

            return Created();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest req)
        {
            var res = await _identity.LoginAsync(req);
            if (!res.IsSuccess) return Unauthorized();

            res.Data.Token = _jwtGenerator.GenerateToken(res.Data);

            return Ok(res.Data);
        }
    }

}
