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

        [HttpGet("users/{id}")]
        public async Task<ActionResult<LoginResponse>> GetUserById(string id)
        {
            var res = await _identity.GetByIdAsync(id);
            if (!res.IsSuccess || res.Data == null) return NotFound(res.ErrorMessage);
            return Ok(res.Data);
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var res = await _identity.DeleteUserAsync(id);
            if (!res.IsSuccess) return BadRequest(res.ErrorMessage);
            return Ok();
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult<LoginResponse>> UpdateUser(string id, [FromBody] UpdateUserRequest req)
        {
            var res = await _identity.UpdateUserAsync(id, req);
            if (!res.IsSuccess) return BadRequest(res.ErrorMessage);
            return Ok(res.Data);
        }

        [HttpPost("users/{id}/change-password")]
        public async Task<ActionResult> ChangePassword(string id, [FromBody] ChangePasswordRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var res = await _identity.ChangePasswordAsync(id, req.NewPassword);
            if (!res.IsSuccess) return BadRequest(res.ErrorMessage);
            return Ok();
        }

        [HttpGet("rings")]
        public async Task<ActionResult<List<RingResponse>>> GetRings()
        {
            // For now, return empty or mock; adjust as needed
            return Ok(new List<RingResponse>());
        }

    }
}
