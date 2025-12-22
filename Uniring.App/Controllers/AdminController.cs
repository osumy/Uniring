using Microsoft.AspNetCore.Mvc;

namespace Uniring.App.Controllers
{
    public class AdminController : Controller
    {
        [Route("admin-panel")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("admin-panel/new-ring")]
        public IActionResult NewRing()
        {
            return View();
        }

        [Route("admin-panel/new-user")]
        public IActionResult NewUser()
        {
            return View();
        }
    }
}
