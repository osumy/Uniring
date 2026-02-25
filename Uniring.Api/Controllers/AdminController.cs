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
        private readonly UniringDbContext _db;

        public AdminController(IIdentityService identity, IRingService ringService, UniringDbContext db)
        {
            _identity = identity;
            _ringService = ringService;
            _db = db;
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
            var rings = await _ringService.GetRingsAsync();
            return Ok(rings);
        }

        [HttpPost("rings")]
        public async Task<ActionResult<RingResponse>> RegisterRing([FromBody] RingRegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Name is required.");

            var ring = new Ring
            {
                Id = Guid.NewGuid(),
                Uid = $"RNG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}",
                Name = req.Name.Trim(),
                Serial = Guid.NewGuid().ToString("N"),
                Description = req.Description?.Trim()
            };

            await _db.Rings.AddAsync(ring);
            await _db.SaveChangesAsync();

            if (req.MediaIds is { Count: > 0 })
            {
                var medias = await _db.Medias
                    .Where(m => req.MediaIds.Contains(m.Id))
                    .ToListAsync();

                foreach (var media in medias)
                {
                    media.RingId = ring.Id;
                }

                await _db.SaveChangesAsync();
            }

            var response = new RingResponse
            {
                Id = ring.Id,
                Uid = ring.Uid,
                Name = ring.Name,
                Serial = ring.Serial,
                Description = ring.Description,
                MediaIds = req.MediaIds ?? new List<Guid>()
            };

            return Ok(response);
        }

    }
}
