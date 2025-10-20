using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Uniring.Contracts.Auth;

namespace Uniring.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _api;

        public AccountController(IApiService api)
        {
            _api = api;
        }

        [Route("login")]
        public IActionResult Login()
        {
            ViewBag.Title = "ورود";
            return View();
        }

        [Route("signup")]
        [HttpGet]
        public IActionResult Signup()
        {
            ViewBag.Title = "ثبت نام";
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(RegisterRequest requestModel)
        {
            ViewBag.Title = "ثبت نام";

            if (!ModelState.IsValid) return View(requestModel);

            var result = await _api.RegisterAsync(requestModel);

            if (result.Token != null) { RedirectToRoute("/"); }

            return View();
        }

        [Route("login-ar")]
        public IActionResult LoginAr()
        {
            return View();
        }

        [Route("signup-ar")]
        public IActionResult SignupAr()
        {
            return View();
        }
    }
}
