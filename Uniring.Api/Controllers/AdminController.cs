using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
using Uniring.Application.Interfaces;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;
using Uniring.Contracts.Invoice;
using Uniring.Domain.Entities;
using Uniring.Infrastructure;

namespace Uniring.Api.Controllers
{
    public class AdminController : ApiControllerBase
    {
        private readonly IIdentityService _identity;
        private readonly IRingService _ringService;
        private readonly IInvoiceService _invoiceService;
        private readonly UniringDbContext _db;

        public AdminController(IIdentityService identity, IRingService ringService, IInvoiceService invoiceService, UniringDbContext db)
        {
            _identity = identity;
            _ringService = ringService;
            _invoiceService = invoiceService;
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

        [HttpGet("users/search")]
        public async Task<ActionResult<List<LoginResponse>>> SearchUsers([FromQuery] string term, [FromQuery] bool includeGuests = true)
        {
            var users = await _identity.SearchUsersAsync(term, includeGuests);
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

        [HttpGet("invoices/recent")]
        public async Task<ActionResult<List<InvoiceResponse>>> GetRecentInvoices([FromQuery] int count = 50)
        {
            var invoices = await _invoiceService.GetRecentInvoicesAsync(count);
            return Ok(invoices);
        }

        [HttpGet("invoices/{id:guid}")]
        public async Task<ActionResult<InvoiceResponse>> GetInvoiceById(Guid id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null) return NotFound();
            return Ok(invoice);
        }

        [HttpPost("invoices")]
        public async Task<ActionResult<InvoiceResponse>> CreateInvoice([FromBody] InvoiceCreateRequest req)
        {
            if (req.RingId == Guid.Empty || req.UserId == Guid.Empty)
                return BadRequest("RingId and UserId are required.");

            var created = await _invoiceService.CreateAsync(req);

            // Update last purchase timestamp
            await _identity.SetLastPurchaseAsync(created.UserId.ToString(), created.CreatedAtUtc);

            return Ok(created);
        }

        [HttpPut("invoices/{id:guid}/owner")]
        public async Task<ActionResult<InvoiceResponse>> ChangeInvoiceOwner(Guid id, [FromBody] InvoiceUpdateRequest req)
        {
            if (req.Id == Guid.Empty)
                req.Id = id;

            if (req.Id != id)
                return BadRequest("Id mismatch.");

            var updated = await _invoiceService.UpdateOwnerAsync(req);
            await _identity.SetLastPurchaseAsync(updated.UserId.ToString(), updated.CreatedAtUtc);
            return Ok(updated);
        }

        [HttpDelete("invoices/{id:guid}")]
        public async Task<ActionResult> DeleteInvoice(Guid id)
        {
            await _invoiceService.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("rings")]
        public async Task<ActionResult<RingResponse>> RegisterRing([FromBody] RingRegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Name is required.");

            string uid;
            string serial;
            bool isUnique = false;
            int retryCount = 0;

            do
            {
                uid = Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 4);
                var now = DateTime.UtcNow;
                serial = $"SH-{now:yy}{now.DayOfYear:D3}-{uid}";

                // Check uniqueness in DB
                var exists = await _db.Rings.AnyAsync(r => r.Uid == uid || r.Serial == serial);
                if (!exists)
                {
                    isUnique = true;
                }
                retryCount++;
            } while (!isUnique && retryCount < 10);

            if (!isUnique)
                return StatusCode(500, "Failed to generate a unique ID for the ring.");

            var ring = new Ring
            {
                Id = Guid.NewGuid(),
                Uid = uid,
                Name = req.Name.Trim(),
                Serial = serial,
                Description = req.Description?.Trim()
            };

            await _db.Rings.AddAsync(ring);
            await _db.SaveChangesAsync();

            if (req.MediaIds is { Count: > 0 })
            {
                var medias = await _db.Medias
                    .Where(m => req.MediaIds.Contains(m.Id))
                    .ToListAsync();

                // Preserve order based on req.MediaIds
                for (int i = 0; i < req.MediaIds.Count; i++)
                {
                    var mediaId = req.MediaIds[i];
                    var media = medias.FirstOrDefault(m => m.Id == mediaId);
                    if (media != null)
                    {
                        media.RingId = ring.Id;
                        media.Order = i;
                    }
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

        [HttpDelete("rings/{id}")]
        public async Task<ActionResult> DeleteRing(Guid id)
        {
            var ring = await _db.Rings.FindAsync(id);
            if (ring == null) return NotFound();

            _db.Rings.Remove(ring);
            await _db.SaveChangesAsync();
            return Ok();
        }

    }
}
