using almondcove.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace almondcove.Controllers
{
    public class HomeRouteController : Controller
    {
        private readonly ILogger<HomeRouteController> _logger;

        public HomeRouteController(ILogger<HomeRouteController> logger)
        {
            _logger = logger;
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View("Views/Home/Index.cshtml");
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View("Views/Home/About.cshtml");
        }

        [Route("/404")]
        public IActionResult NF()
        {
            return View("Views/Home/404.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}