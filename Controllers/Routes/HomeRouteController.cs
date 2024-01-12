using Microsoft.AspNetCore.Mvc;

namespace almondcove.Controllers.Routes
{
    public class HomeRouteController() : Controller
    {
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

        [Route("/401")]
        public IActionResult UA()
        {
            return View("Views/Home/AccessDenied.cshtml");
        }
    }
}