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
        public async Task<IActionResult> Index()
        {
            var user = await _api.GetRingByUidAsync("UID");
            ViewBag.user = user;

            return View();
        }

        [Route("/s/{serial}")]
        public async Task<IActionResult> Index(string serial)
        {
            var user = await _api.GetRingBySerialAsync(serial);
            ViewBag.user = user;

            return View();
        }
    }
}
