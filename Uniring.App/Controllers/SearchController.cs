using Microsoft.AspNetCore.Mvc;
using Uniring.App.Interfaces;

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

        [Route("/{identifier}")]
        public async Task<IActionResult> Index(string identifier)
        {
            // Try by Uid first
            var ring = await _api.GetRingByUidAsync(identifier);
            
            // If not found, try by Serial
            if (ring == null)
            {
                ring = await _api.GetRingBySerialAsync(identifier);
            }

            if (ring == null)
            {
                return View("Index"); // Show search page if not found
            }

            return View("Result", ring);
        }

        [HttpGet("api/Ring/{identifier}")]
        public async Task<IActionResult> GetRingByIdentifier(string identifier)
        {
            var ring = await _api.GetRingByUidAsync(identifier);
            if (ring == null) ring = await _api.GetRingBySerialAsync(identifier);

            return ring == null ? NotFound() : Ok(ring);
        }
    }
}
