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

            // TODO JWT
            //var token = _jwtGenerator.GenerateToken(result.Data);
            //var response = new loginResponseDto
            //{
            //    PhoneNumber = result.Data.PhoneNumber,
            //    Roles = result.Data.Roles,
            //    Token = token
            //};
             
            //return Ok(response);

            return Created();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest req)
        {
            var res = await _identity.LoginAsync(req);
            if (!res.Success) return Unauthorized();

            var token = _jwtGenerator.GenerateToken(res);

            var response = new LoginResponse
            {
                Id = res.Id,
                Token = token,
                PhoneNumber = res.PhoneNumber,
                Role = res.Role
            };

            return Ok(response);
        }
    }

}
