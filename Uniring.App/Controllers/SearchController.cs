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

        public IActionResult Index()
        {

            return View();
        }
    }
}
