using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uniring.Application.Interfaces;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;

namespace Uniring.Api.Controllers
{
    public class AdminController : ApiControllerBase
    {
        private readonly IIdentityService _identity;
        private readonly IRingService _ringService;

        public AdminController(IIdentityService identity, IRingService ringService)
        {
            _identity = identity;
            _ringService = ringService;
        }

        [HttpPost("register-user")]
        public async Task<ActionResult> Register(RegisterRequest req)
        {
            var res = await _identity.RegisterCustomerAsync(req);
            if (!res.IsSuccess) return BadRequest(res.ErrorMessage);

            return Ok();
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<LoginResponse>>> GetUsers()
        {
            var users = await _identity.GetUsersInRoleAsync("user"); // Only "user" role
            return Ok(users);
        }

        [HttpGet("rings")]
        public async Task<ActionResult<List<RingResponse>>> GetRings()
        {
            // For now, return empty or mock; adjust as needed
            return Ok(new List<RingResponse>());
        }

    }
}
