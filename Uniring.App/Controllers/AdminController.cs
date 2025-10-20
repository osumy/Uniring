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
    }
}
