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

        //////////////////////////////////////////////////////////////////////////
        // Login
        //////////////////////////////////////////////////////////////////////////

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
            ViewBag.Title = "ورود";

            return RedirectToAction("Index", "Admin");

            if (requestModel.PhoneNumber == "09919529364" && requestModel.Password == "passpass1516")
            {
                return RedirectToAction("Index", "AdminController");
            }

            return NotFound();
        }

        [Route("login-ar")]
        [HttpGet]
        public IActionResult LoginAr(LoginRequest requestModel)
        {
            ViewBag.Title = "تسجيل الدخول";
            return View();
        }

        [Route("login-ar")]
        [HttpPost]
        public IActionResult LoginAr()
        {
            ViewBag.Title = "تسجيل الدخول";
            return View();
        }

        //////////////////////////////////////////////////////////////////////////
        // Signup
        //////////////////////////////////////////////////////////////////////////

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

        [Route("signup-ar")]
        [HttpGet]
        public IActionResult SignupAr()
        {
            ViewBag.Title = "إنشاء حساب";
            return View();
        }

        [Route("signup-ar")]
        [HttpPost]
        public IActionResult SignupAr(RegisterRequest requestModel)
        {
            ViewBag.Title = "إنشاء حساب";
            return View();
        }
    }
}
