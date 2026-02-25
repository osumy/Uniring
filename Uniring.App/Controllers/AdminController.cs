using Microsoft.AspNetCore.Mvc;
using Uniring.App.Interfaces;
using Uniring.Contracts.Auth;
using Uniring.Contracts.Ring;

namespace Uniring.App.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminApiService _api;

        public AdminController(IAdminApiService api)
        {
            _api = api;
        }

        [Route("admin-panel")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("admin-panel/new-ring")]
        [HttpGet]
        public IActionResult NewRing()
        {
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
            if (!ModelState.IsValid) return View(registerRequest);

            var result = await _api.CreateNewAccountAsync(registerRequest);

            if (result)
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Error = "ثبت ناموفق!";
            return View();
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
