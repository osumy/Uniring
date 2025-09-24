using Microsoft.AspNetCore.Mvc;

namespace Uniring.App.Controllers
{
    public class AccountController : Controller
    {

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("signup")]
        public IActionResult Signup()
        {
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
