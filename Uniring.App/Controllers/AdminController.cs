using Microsoft.AspNetCore.Mvc;
using Uniring.App.Interfaces;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;
using Uniring.Contracts.Invoice;

namespace Uniring.App.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminApiService _api;
        private readonly IConfiguration _configuration;

        public AdminController(IAdminApiService api, IConfiguration configuration)
        {
            _api = api;
            _configuration = configuration;
        }

        [Route("admin-panel")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("admin-panel/new-invoice")]
        [HttpGet]
        public IActionResult NewInvoice()
        {
            ViewBag.ApiBaseUrl = _configuration["Api:BaseUrl"] ?? string.Empty;
            return View();
        }

        [HttpGet("admin-panel/invoices/{id}/edit")]
        public IActionResult EditInvoice(Guid id)
        {
            ViewBag.ApiBaseUrl = _configuration["Api:BaseUrl"] ?? string.Empty;
            ViewBag.InvoiceId = id;
            return View();
        }

        [Route("admin-panel/new-ring")]
        [HttpGet]
        public IActionResult NewRing()
        {
            ViewBag.ApiBaseUrl = _configuration["Api:BaseUrl"] ?? string.Empty;
            return View();
        }

        [Route("admin-panel/new-ring")]
        [HttpPost]
        public IActionResult NewRing(RingResponse ringResponse)
        {
            return View();
        }

        [Route("admin-panel/new-user")]
        [HttpGet]
        public IActionResult NewUser()
        {
            return View();
        }

        [Route("admin-panel/new-user")]
        [HttpPost]
        public async Task<IActionResult> NewUser(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _api.CreateNewAccountAsync(registerRequest);

            if (result)
            {
                return Ok(new { success = true });
            }

            return BadRequest(new { success = false, message = "ثبت ناموفق!" });
        }

        [HttpGet("admin-panel/users/{id}/edit")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _api.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var model = new UpdateUserRequest
            {
                DisplayName = user.DisplayName,
                PhoneNumber = user.PhoneNumber
            };

            ViewBag.UserId = id;
            return View(model);
        }

        [HttpPost("admin-panel/users/{id}/edit")]
        public async Task<IActionResult> EditUser(string id, UpdateUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = id;
                return View(model);
            }

            var ok = await _api.UpdateAccountAsync(id, model);
            if (!ok)
            {
                ViewBag.UserId = id;
                ViewBag.Error = "بروزرسانی ناموفق بود.";
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("admin-panel/users/{id}/change-password")]
        public IActionResult ChangePassword(string id)
        {
            ViewBag.UserId = id;
            return View(new ChangePasswordRequest());
        }

        [HttpPost("admin-panel/users/{id}/change-password")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = id;
                return View(model);
            }

            var ok = await _api.ChangePasswordAsync(id, model);
            if (!ok)
            {
                ViewBag.UserId = id;
                ViewBag.Error = "تغییر رمز عبور ناموفق بود.";
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("api/users")]
        public async Task<IActionResult> GetUsersJson()
        {
            var users = await _api.GetUsersAsync();
            return Json(users);
        }

        [HttpDelete("api/users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var ok = await _api.DeleteAccountAsync(id);
            if (!ok) return BadRequest();
            return Ok();
        }

        [HttpGet("api/rings")]
        public async Task<IActionResult> GetRingsJson()
        {
            var rings = await _api.GetRingsAsync();
            return Json(rings);
        }

        [HttpDelete("api/rings/{id}")]
        public async Task<IActionResult> DeleteRing(string id)
        {
            var ok = await _api.DeleteRingAsync(id);
            if (!ok) return BadRequest();
            return Ok();
        }

        [HttpGet("api/invoices/recent")]
        public async Task<IActionResult> GetRecentInvoicesJson()
        {
            var invoices = await _api.GetRecentInvoicesAsync(100);
            return Json(invoices);
        }

        [HttpDelete("api/invoices/{id}")]
        public async Task<IActionResult> DeleteInvoice(string id)
        {
            if (!Guid.TryParse(id, out var guid)) return BadRequest();
            var ok = await _api.DeleteInvoiceAsync(guid);
            if (!ok) return BadRequest();
            return Ok();
        }

        [HttpPut("api/users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _api.UpdateAccountAsync(id, model);
            if (!ok) return BadRequest();

            // برای سادگی، همان مدل ورودی را برمی‌گردانیم
            return Json(model);
        }
    }
}
