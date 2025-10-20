using Microsoft.AspNetCore.Mvc;
using System.Numerics;
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
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "ورود";
            return View();
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginRequest requestModel)
        {
            return RedirectToAction("Index", "Admin");

            if (requestModel.PhoneNumber == "09919529364" && requestModel.Password == "passpass1516")
            {
                return RedirectToAction("Index", "AdminController");
            }

            return NotFound();
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
