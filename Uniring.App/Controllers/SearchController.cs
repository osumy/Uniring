using Microsoft.AspNetCore.Mvc;

namespace Uniring.App.Controllers
{
    public class SearchController : Controller
    {
        private readonly IApiService _api;

        public SearchController(IApiService api)
        {
            _api = api;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/serial/{serial}")]
        public async Task<IActionResult> Index(string serial)
        {
            var user = await _api.GetRingBySerialAsync(serial);
            ViewBag.user = user;

            return Ok(user);
        }
    }
}
