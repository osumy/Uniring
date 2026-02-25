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
