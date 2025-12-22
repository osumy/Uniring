using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Reflection;
using Uniring.App.Interfaces;
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
        public async Task<IActionResult> Login(LoginRequest requestModel)
        {
            ViewBag.Title = "ورود";

            if (!ModelState.IsValid) return View(requestModel);

            var result = await _api.LoginAsync(requestModel);

            if (result?.Token != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("authToken", result.Token, cookieOptions);
                Response.Cookies.Append("userId", result.Id, cookieOptions);
                Response.Cookies.Append("userName", result.DisplayName, cookieOptions);
                Response.Cookies.Append("phoneNumber", result.PhoneNumber, cookieOptions);
                Response.Cookies.Append("userRole", result.Role, cookieOptions);

                return RedirectToAction("Index", "Search");
            }
            return View();

            //return RedirectToAction("Index", "Admin");
        }

        [Route("login-ar")]
        [HttpGet]
        public IActionResult LoginAr()
        {
            ViewBag.Title = "تسجيل الدخول";
            return View();
        }

        [Route("login-ar")]
        [HttpPost]
        public async Task<IActionResult> LoginAr(LoginRequest requestModel)
        {
            ViewBag.Title = "تسجيل الدخول";

            if (!ModelState.IsValid) return View(requestModel);

            var result = await _api.LoginAsync(requestModel);

            if (result?.Token != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("authToken", result.Token, cookieOptions);
                Response.Cookies.Append("userId", result.Id, cookieOptions);
                Response.Cookies.Append("userName", result.DisplayName, cookieOptions);
                Response.Cookies.Append("phoneNumber", result.PhoneNumber, cookieOptions);
                Response.Cookies.Append("userRole", result.Role, cookieOptions);

                return RedirectToAction("Index", "Search");
            }
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

            if (result?.Token != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("authToken", result.Token, cookieOptions);
                Response.Cookies.Append("userId", result.Id, cookieOptions);
                Response.Cookies.Append("userName", result.DisplayName, cookieOptions);
                Response.Cookies.Append("phoneNumber", result.PhoneNumber, cookieOptions);
                Response.Cookies.Append("userRole", result.Role, cookieOptions);

                return RedirectToAction("Index", "Search");
            }
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
        public async Task<IActionResult> SignupAr(RegisterRequest requestModel)
        {
            ViewBag.Title = "إنشاء حساب";

            if (!ModelState.IsValid) return View(requestModel);

            var result = await _api.RegisterAsync(requestModel);

            if (result?.Token != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("authToken", result.Token, cookieOptions);
                Response.Cookies.Append("userId", result.Id, cookieOptions);
                Response.Cookies.Append("userName", result.DisplayName, cookieOptions);
                Response.Cookies.Append("phoneNumber", result.PhoneNumber, cookieOptions);
                Response.Cookies.Append("userRole", result.Role, cookieOptions);

                return RedirectToAction("Index", "Search");
            }
            return View();
        }
    }
}
